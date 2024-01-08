using System.Collections.Generic;
using Core;
using Data.MilitaryGame;
using UnityEngine;

namespace MilitaryGame.UI.ProductionMenu
{
    public class ProductMenuController : BaseMonoBehaviour
    {
        #region Variable Fields
        
        [Header("BUILDING SLOT")]
        [SerializeField] private BuildingSlotButton _buildingSlotButtonPref;
        [SerializeField] private Transform _contentParent;

        private List<BuildingSlotButton> _buildingSlotButtonList = new();
        private bool _isMenuOpen;

        private List<BuildingData> _buildingDataList = new();
        
        #endregion // Variable Fields
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _buildingDataList = (List<BuildingData>) list[0];
            CreateBuildingSlotButtons();
        }
    
        /// <summary>
        /// Creates and initializes product slot buttons for each building in the building data list.
        /// </summary>
        private void CreateBuildingSlotButtons()
        {
            for (int i = 0; i < _buildingDataList.Count; i++)
            {
                BuildingSlotButton buildingSlotButton = Instantiate(_buildingSlotButtonPref, _contentParent);

                buildingSlotButton.Initialize(_buildingDataList[i]);
            
                _buildingSlotButtonList.Add(buildingSlotButton);
            }
        }

        private void ClearAllBuildingSlotButton()
        {
            foreach (BuildingSlotButton slotButton in _buildingSlotButtonList)
            {
                if(slotButton == null) return;
            
                slotButton.End();
                Destroy(slotButton.gameObject);
            }
            _buildingSlotButtonList.Clear();
        }

        public override void End()
        {
            base.End();
            ClearAllBuildingSlotButton();
        }
    }
}
