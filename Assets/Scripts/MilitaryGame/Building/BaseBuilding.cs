using Core;
using Interfaces.MilitaryGame;
using MilitaryGame.GridBuilding;
using MilitaryGame.UI.HealthBar;
using UnityEngine;

namespace MilitaryGame.Building
{
    public class BaseBuilding : BaseMonoBehaviour, ILeftClickable, IDamageable, IRightClickable
    {
        [SerializeField] private protected BuildingData _buildingData;
        [SerializeField] private BoundsInt _area;

        [Header("HEALTH BAR")] 
        [SerializeField] protected HealthBar _healthBar;
        
        public bool Placed { get; protected set; }
        public BoundsInt Area
        {
            get => _area;
            set => _area = value;
        }
        public BuildingData BuildingData => _buildingData;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
            
            ActivateHealthBar();
        }
        
        public override void End()
        {
            base.End();
            
            Placed = false;
            _healthBar.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the health bar, initializing it with the building's maximum health points.
        /// </summary>
        private void ActivateHealthBar()
        {
            _healthBar.gameObject.SetActive(true);
            _healthBar.Initialize((float)_buildingData.HealthPoint);
        }
        
        public void TakeDamage(int damage) { }

        #region CLICK METHODS

        public void OnLeftClick()
        {
            if(!Placed) return;
            
            MilitaryGameEventLib.Instance.ShowBuildingInfo(this);
        }
        
        public void OnRightClick() { }

        #endregion

        #region BUILD PLACEMENT METHODS
        
        /// <summary>
        /// Checks if the object can be placed on the grid based on its current position.
        /// </summary>
        /// <returns>Returns true if the object can be placed, otherwise false.</returns>
        public bool CanBePlaced()
        {
            // Convert the world position to grid cell coordinates.
            Vector3Int positionInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(transform.position);

            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;

            // Check if the area is available on the grid.
            return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
        }

        /// <summary>
        /// Places the object on the grid, updates the occupied area, and initializes the placed object.
        /// </summary>
        public void Place()
        {
            // Convert the world position to grid cell coordinates.
            Vector3Int positionInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(transform.position);

            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;

            // Mark the object as placed and update the occupied area on the grid.
            Placed = true;
            GridBuildingSystem.Instance.TakeArea(areaTemp);

            Initialize();
        }


        #endregion
    }
}
