using System.Collections;
using System.Collections.Generic;
using Core;
using Data.MilitaryGame;
using DG.Tweening;
using Interface;
using MilitaryGame.Factory;
using MilitaryGame.GridBuilding;
using MilitaryGame.UI.HealthBar;
using UnityEngine;

namespace MilitaryGame.Soldier
{
    public class Soldier : BaseMonoBehaviour, ILeftClickable, IAttackable, IDamageable, IRightClickable
    {
        #region Variable Fields

        [SerializeField] private SoldierData _soldierData;
        [SerializeField] private HealthBar _healthBar;

        [Header("MOVEMENT")]
        [SerializeField] private float _moveDuration;
        
        [Header("INDICATOR")]
        [SerializeField] private GameObject _selectedIndicatorObj;

        public SoldierData SoldierData => _soldierData;
        
        private float _currentHealthPoint;
        public bool IsAttacking { get; set; }
        
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _selectedIndicatorObj.SetActive(value);
            }
        }

        #endregion // Variable Fields

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
            
            _currentHealthPoint = _soldierData.HealthPoint;
            _healthBar.Initialize(_currentHealthPoint);
        }

        private void Update()
        {
            if (!IsSelected || IsAttacking) return;

            if (Input.GetMouseButtonDown(1))
            {
                IDamageable damageableObject = MilitaryGameEventLib.Instance.GetCurrentDamageableObject?.Invoke();
                
                if (damageableObject != null)
                {
                    Attack(damageableObject);
                }
                
                Deselect();
                CheckMovement();
            }
        }
        
        #region CLICK METHODS
        
        public void OnLeftClick()
        {
            if (!IsSelected && !IsAttacking)
            {
                Select();
            }
        }
        
        public void OnRightClick()
        {
            if (!IsSelected)
            {
                MilitaryGameEventLib.Instance.SetDamageableObject?.Invoke(this);
            }
        }
        
        #endregion

        #region SELECT / DESELECT METHODS

        private void Select()
        {
            IsSelected = true;
            MilitaryGameEventLib.Instance.AddSelectedSoldier?.Invoke(this);
        }

        private void Deselect()
        {
            IsSelected = false;
            MilitaryGameEventLib.Instance.RemoveSelectedSoldier?.Invoke(this);
        }

        #endregion

        #region MOVEMENT TRANSACTIONS
        
        /// <summary>
        /// Checks the movement by finding a path from the current position to the mouse cursor position.
        /// </summary> 
        private void CheckMovement()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            // Convert the current position and target position to grid cell positions.
            Vector3Int startCellPos = GridBuildingSystem.Instance.MainTilemap.WorldToCell(transform.position);
            Vector3Int endCellPos = GridBuildingSystem.Instance.MainTilemap.WorldToCell(mouseWorldPos);

            // Find a path from the current position to the target position using the Pathfinder.
            List<Vector3Int> path = Pathfinder.Pathfinder.Instance.FindPath(startCellPos, endCellPos);
            
            StartCoroutine(MoveOnPathCoroutine(path));
        }
        
        /// <summary>
        /// Moves the object along the given path using the Unity coroutine system.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        private IEnumerator MoveOnPathCoroutine(List<Vector3Int> path)
        {
            int index = 0;
            
            while (path.Count > 0 && index < path.Count && IsAlive())
            {
                Vector3 targetWorldPos = GridBuildingSystem.Instance.MainTilemap.GetCellCenterWorld(path[index]);
                
                transform.DOMove(targetWorldPos, _moveDuration);
                index++;

                yield return new WaitForSeconds(_moveDuration);
            }
        }
        
        #endregion

        #region DAMAGE TRANSACTIONS
        
        public bool IsAlive()
        {
            return _currentHealthPoint > 0;
        }

        /// <summary>
        /// Inflicts damage to the soldier and updates the health bar. Destroys the soldier if health reaches zero.
        /// </summary>
        /// <param name="damage">Amount of damage to inflict.</param>
        public void TakeDamage(int damage)
        {
            _currentHealthPoint -= damage;
            _healthBar.SetHealthBar(_currentHealthPoint);
            
            if (!IsAlive())
            {
                End();
                SoldierFactory.Instance.DestroySoldier(this);
            }
        }
        
        #endregion

        #region ATTACK METHODS
        
        /// <summary>
        /// Initiates an attack on a damageable object.
        /// </summary>
        /// <param name="damageableObject">The damageable object to attack.</param>
        public void Attack(IDamageable damageableObject)
        {
            IsAttacking = true;
            StartCoroutine(AttackCoroutine(damageableObject));
        }

        /// <summary>
        /// Coroutine for handling the attack process over time.
        /// </summary>
        /// <param name="damageableObject">The damageable object to attack.</param>
        private IEnumerator AttackCoroutine(IDamageable damageableObject)
        {
            while (damageableObject.IsAlive())
            {
                damageableObject.TakeDamage(_soldierData.DamagePoint);
                yield return new WaitForSeconds(1f);
            }
            
            // Notify listeners that the attack has ended.
            MilitaryGameEventLib.Instance.SetDamageableObject?.Invoke(null);
            IsAttacking = false;
        }

        #endregion
        
        public override void End()
        {
            base.End();
            
            Deselect();
            IsAttacking = false;
        }
    }
}