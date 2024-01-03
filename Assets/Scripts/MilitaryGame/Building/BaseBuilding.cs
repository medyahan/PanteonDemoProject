using Core;
using UnityEngine;

namespace MilitaryGame.Building
{
    public class BaseBuilding : BaseMonoBehaviour, ILeftClickable
    {
        [SerializeField] private protected BuildingData _buildingData;
        [SerializeField] private BoundsInt _area;

        public bool Placed { get; private set; }
        public BoundsInt Area => _area;
        public BuildingData BuildingData => _buildingData;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            // health bar ekle
        }
    
        public void OnLeftClick()
        {
            if(!Placed) return;
        
            MilitaryGameEventLib.Instance.ShowBuildingInfo(this);
        }

        #region BUILD PLACEMENT METHODS

        public bool CanBePlaced()
        {
            //if (Placed) return false;
            
            Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;
        
            if (GridBuildingSystem.Current.CanTakeArea(areaTemp))
            {
                return true;
            }

            return false;
        }

        public void Place()
        {
            Vector3Int positionInt = GridBuildingSystem.Current.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = _area;
            areaTemp.position = positionInt;
            Placed = true;
            GridBuildingSystem.Current.TakeArea(areaTemp);
        
            Initialize();
        }

        #endregion
    }
}
