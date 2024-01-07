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
    public class Soldier : BaseMonoBehaviour, ILeftClickable, IAttackable, IDamageable
    {
        #region Variable Fields
        
        [SerializeField] private SoldierData _soldierData;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        public SoldierData SoldierData => _soldierData;

        private Tilemap _tilemap;
        private Vector3Int _targetPosition;
        
        private bool _isSelected;
        private float _currentHealthPoint;
        
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
            if(!_isSelected) return;
        
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //_targetPosition = _tilemap.WorldToCell(mouseWorldPos);
                
                //Vector3Int soldierPos = _tilemap.WorldToCell(transform.position);
                
                Vector3Int startCellPos = _tilemap.WorldToCell(transform.position);
                Vector3Int endCellPos = _tilemap.WorldToCell(mouseWorldPos);
                
                // Find a path from the current position to the target position and move along the path.
                List<Vector3Int> path = Pathfinder.Pathfinder.Instance.FindPath(startCellPos, endCellPos);
                MoveOnPath(path);
            }
        }
        
        public void TakeDamage(int damage)
        {
            _currentHealthPoint -= damage;
            _healthBar.SetHealthBar(_currentHealthPoint);
        }
        
        public void Attack()
        {
            // BaseBuilding building;
            // building.TakeDamage(_soldierData.DamagePoint);
        }

        /// <summary>
        /// Moves the agent along the specified path.
        /// </summary>
        /// <param name="path">The list of cell positions representing the path.</param>
        private void MoveOnPath(List<Vector3Int> path)
        {
            Debug.Log("path count" + path.Count);
            if (path.Count > 0)
            {
                Vector3 targetWorldPos = _tilemap.GetCellCenterWorld(path[path.Count - 1]);
                transform.DOMove(targetWorldPos, .4f);
                //_navMeshAgent.SetDestination(targetWorldPos);
            }
        }
    
        public void OnLeftClick()
        {
            _isSelected = true;
        }

        public override void End()
        {
            base.End();
            _isSelected = false;
        }
    }
}
