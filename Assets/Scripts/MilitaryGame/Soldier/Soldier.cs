using System.Collections;
using System.Collections.Generic;
using Core;
using Data.MilitaryGame;
using DG.Tweening;
using Interfaces.MilitaryGame;
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
                    MilitaryGameEventLib.Instance.SetDamageableObject?.Invoke(null);
                }
                IsSelected = false;
                Move();
            }
        }

        #region MOVEMENT TRANSACTIONS
        
        private void Move()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            Vector3Int startCellPos = GridBuildingSystem.Instance.MainTilemap.WorldToCell(transform.position);
            Vector3Int endCellPos = GridBuildingSystem.Instance.MainTilemap.WorldToCell(mouseWorldPos);

            // Find a path from the current position to the target position and move along the path.
            List<Vector3Int> path = Pathfinder.Pathfinder.Instance.FindPath(startCellPos, endCellPos);
            StartCoroutine(MoveOnPathCoroutine(path));
        }
        
        /// <summary>
        /// Moves the agent along the specified path.
        /// </summary>
        /// <param name="path">The list of cell positions representing the path.</param>
        private IEnumerator MoveOnPathCoroutine(List<Vector3Int> path)
        {
            int index = 0;
            
            while (path.Count > 0 && index < path.Count)
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
        
        #region CLICK METHODS
        
        public void OnLeftClick()
        {
            if(!IsAttacking)
                IsSelected = true;
        }
        
        public void OnRightClick()
        {
            if (!IsSelected)
            {
                MilitaryGameEventLib.Instance.SetDamageableObject?.Invoke(this);
            }
        }
        
        #endregion

        #region ATTACK METHODS
        
        public void Attack(IDamageable damageableObject)
        {
            IsAttacking = true;
            StartCoroutine(AttackCoroutine(damageableObject));
        }

        private IEnumerator AttackCoroutine(IDamageable damageableObject)
        {
            while (damageableObject.IsAlive())
            {
                damageableObject.TakeDamage(_soldierData.DamagePoint);
                yield return new WaitForSeconds(1f);
            }

            IsAttacking = false;
        }

        #endregion
        
        public override void End()
        {
            base.End();
            IsSelected = false;
            IsAttacking = false;
        }
    }
}