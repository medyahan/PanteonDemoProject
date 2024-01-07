using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using Interfaces.MilitaryGame;
using MilitaryGame.Building;
using MilitaryGame.GridBuilding;
using MilitaryGame.UI.HealthBar;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace MilitaryGame.Soldier
{
    public class Soldier : BaseMonoBehaviour, ILeftClickable, IAttackable, IDamageable, IRightClickable
    {
        #region Variable Fields

        [SerializeField] private SoldierData _soldierData;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private GameObject _selectedObj;

        public SoldierData SoldierData => _soldierData;

        private Tilemap _tilemap;
        private Vector3Int _targetPosition;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _selectedObj.SetActive(value);
            }
        }

        private float _currentHealthPoint;
        public bool IsAttacking { get; set; }

        private BaseBuilding _building;

        #endregion // Variable Fields

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
            _currentHealthPoint = _soldierData.HealthPoint;
            _healthBar.Initialize(_currentHealthPoint);
            _tilemap = GridBuildingSystem.Instance.MainTilemap;
        }

        private void Update()
        {
            if (!IsSelected) return;

            if (Input.GetMouseButtonDown(1))
            {
                
                // BaseBuilding building = MilitaryGameEventLib.Instance.GetSelectedBuildingForAttack?.Invoke();
                //
                // if (building != null)
                // {
                //     Attack(building);
                // }
                Move();
            }
        }

        private void Move()
        {
            IsSelected = false;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            Vector3Int startCellPos = _tilemap.WorldToCell(transform.position);
            Vector3Int endCellPos = _tilemap.WorldToCell(mouseWorldPos);

            // Find a path from the current position to the target position and move along the path.
            List<Vector3Int> path = Pathfinder.Pathfinder.Instance.FindPath(startCellPos, endCellPos);
            StartCoroutine(MoveOnPath(path));
        }
        
        public void OnLeftClick()
        {
            if(!IsAttacking)
                IsSelected = true;
        }
        
        public void OnRightClick()
        {
            if (!IsSelected)
            {
                // bu asker hasar görecek
            }
        }

        public void TakeDamage(int damage)
        {
            _currentHealthPoint -= damage;
            _healthBar.SetHealthBar(_currentHealthPoint);
        }

        public void Attack(IDamageable damageableObject)
        {
            IsAttacking = true;
            StartCoroutine(AttackCoroutine(damageableObject));
        }

        private IEnumerator AttackCoroutine(IDamageable damageableObject)
        {
            while (damageableObject != null)
            {
                damageableObject.TakeDamage(_soldierData.DamagePoint);
                yield return new WaitForSeconds(1f);
            }

            IsAttacking = false;
        }

        /// <summary>
        /// Moves the agent along the specified path.
        /// </summary>
        /// <param name="path">The list of cell positions representing the path.</param>
        private IEnumerator MoveOnPath(List<Vector3Int> path)
        {
            int index = 0;
            while (path.Count > 0 && index < path.Count)
            {
                Vector3 targetWorldPos = _tilemap.GetCellCenterWorld(path[index]);
                transform.DOMove(targetWorldPos, .2f);
                index++;
                yield return new WaitForSeconds(.2f);
            }
        }
        
        public override void End()
        {
            base.End();
            IsSelected = false;
        }
    }
}