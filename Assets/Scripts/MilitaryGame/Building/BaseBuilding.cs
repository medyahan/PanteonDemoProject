using Core;
using UnityEngine;

namespace MilitaryGame.Building
{
    public class BaseBuilding : BaseMonoBehaviour, ILeftClickable
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

        private void ActivateHealthBar()
        {
            _healthBar.gameObject.SetActive(true);
            _healthBar.Initialize((float)_buildingData.HealthPoint);
        }
        
        public void OnLeftClick()
        {
            if(!Placed) return;
            
            MilitaryGameEventLib.Instance.ShowBuildingInfo(this);
        }
        
        public override void End()
        {
            base.End();
            
            Placed = false;
            _healthBar.gameObject.SetActive(false);
        }

        #region BUILD PLACEMENT METHODS
        
        public bool CanBePlaced()
        {
            Vector3Int positionInt = GridBuildingSystem.GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;
        
            if (GridBuildingSystem.GridBuildingSystem.Current.CanTakeArea(areaTemp))
            {
                return true;
            }
            
            return false;
        }
        
        public void Place()
        {
            Vector3Int positionInt = GridBuildingSystem.GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;
            Placed = true;
            GridBuildingSystem.GridBuildingSystem.Current.TakeArea(areaTemp);
        
            Initialize();
        }

        #endregion
    }
}
