using Core;
using MilitaryGame.Building;
using MilitaryGame.Factory;
using MilitaryGame.UI.ProductionMenu;
using UnityEngine;

namespace MilitaryGame
{
    public class MilitaryGameManager : BaseMonoBehaviour
    {
        #region Variable Fields
    
        [Header("UI CONTROLLERS")]
        [SerializeField] private ProductMenuController _productMenuController;
        [SerializeField] private InformationPanelController _informationPanelController;

        private Soldier.Soldier _selectedSoldier;
        private BaseBuilding _selectedBuilding;

        #endregion // Variable Fields
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            _productMenuController.Initialize();
            _informationPanelController.Initialize();
        
            SoldierFactory.Instance.Initialize();
            BuildingFactory.Instance.Initialize();
        }

        public override void RegisterEvents()
        {
            MilitaryGameEventLib.Instance.ShowBuildingInfo += _informationPanelController.ShowBuildingInfo;
            MilitaryGameEventLib.Instance.CloseInformationPanel += _informationPanelController.Close;
            MilitaryGameEventLib.Instance.SetSelectedBuildingForAttack += OnSetSelectedBuildingForAttack;
            MilitaryGameEventLib.Instance.GetSelectedBuildingForAttack += OnGetSelectedBuildingForAttack;
        }

        public override void UnregisterEvents()
        {
            MilitaryGameEventLib.Instance.ShowBuildingInfo -= _informationPanelController.ShowBuildingInfo;
            MilitaryGameEventLib.Instance.CloseInformationPanel -= _informationPanelController.Close;
            MilitaryGameEventLib.Instance.SetSelectedBuildingForAttack -= OnSetSelectedBuildingForAttack;
            MilitaryGameEventLib.Instance.GetSelectedBuildingForAttack -= OnGetSelectedBuildingForAttack;

        }

        private BaseBuilding OnGetSelectedBuildingForAttack()
        {
            return _selectedBuilding;
        }

        private void OnSetSelectedBuildingForAttack(BaseBuilding building)
        {
            _selectedBuilding = building;
        }

        public override void End()
        {
            MilitaryGameEventLib.Instance.OnDestroy();
            
            base.End();
        
            _productMenuController.End();
            _informationPanelController.End();
        }
    }
}
