using System.Collections.Generic;
using Core;
using Interfaces.MilitaryGame;
using MilitaryGame.GridBuilding;
using MilitaryGame.UI.HealthBar;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

namespace MilitaryGame.Soldier
{
    public class Soldier : BaseMonoBehaviour, ILeftClickable, IAttackable
    {
        #region Variable Fields
        
        [SerializeField] private SoldierData _soldierData;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        public SoldierData SoldierData => _soldierData;
    
        private int _healthPoint;

        private bool _isSelected;
    
        private Tilemap _tilemap;
        private Vector3Int _targetPosition;
        
        #endregion // Variable Fields
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
            _healthBar.Initialize((float)_soldierData.HealthPoint);
        
            _tilemap = GridBuildingSystem.Instance.MainTilemap;
        }
    
        private void Update()
        {
            if(!_isSelected) return;
        
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetPosition = _tilemap.WorldToCell(mouseWorldPos);
            
                // Find a path from the current position to the target position and move along the path.
                List<Vector3Int> path = Pathfinder.Pathfinder.Instance.FindPath(_tilemap.WorldToCell(transform.position), _targetPosition);
                MoveOnPath(path);
            }
        }

        /// <summary>
        /// Moves the agent along the specified path.
        /// </summary>
        /// <param name="path">The list of cell positions representing the path.</param>
        private void MoveOnPath(List<Vector3Int> path)
        {
            if (path.Count > 0)
            {
                Vector3 targetWorldPos = _tilemap.GetCellCenterWorld(path[path.Count - 1]);
                _navMeshAgent.SetDestination(targetWorldPos);
            }
        }
    
        public void OnLeftClick()
        {
            _isSelected = true;
        }

        public void Attack() { }
    
        public override void End()
        {
            base.End();
            _isSelected = false;
        }
    }
}
