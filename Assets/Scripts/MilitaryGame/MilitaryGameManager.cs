using System.Collections.Generic;
using Core;
using Data.MilitaryGame;
using Interface;
using MilitaryGame.Building;
using MilitaryGame.Factory;
using MilitaryGame.UI.ProductionMenu;
using UnityEngine;

namespace MilitaryGame
{
    public class MilitaryGameManager : BaseMonoBehaviour
    {
        #region Variable Fields

        [SerializeField] private MilitaryGameData _militaryGameData;
    
        [Header("UI CONTROLLERS")]
        [SerializeField] private ProductMenuController _productMenuController;
        [SerializeField] private InformationPanelController _informationPanelController;

        private IDamageable _currenDamageableObject;
        private List<Soldier.Soldier> _selectedSoldier = new List<Soldier.Soldier>();

        #endregion // Variable Fields
    
        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        
            _productMenuController.Initialize(_militaryGameData.BuildingDataList);
            _informationPanelController.Initialize(_militaryGameData.SoldierDataList);
        
            SoldierFactory.Instance.Initialize(_militaryGameData.SoldierDataList);
            BuildingFactory.Instance.Initialize(_militaryGameData.BuildingDataList);
        }

        public override void RegisterEvents()
        {
            MilitaryGameEventLib.Instance.ShowBuildingInfo += OnShowBuildingInfo;
            MilitaryGameEventLib.Instance.CloseInformationPanel += OnCloseInformationPanel;
            MilitaryGameEventLib.Instance.SetDamageableObject += OnSetDamageableObject;
            MilitaryGameEventLib.Instance.GetCurrentDamageableObject += OnGetCurrentDamageableObject;
            MilitaryGameEventLib.Instance.AddSelectedSoldier += OnAddSelectedSoldier;
            MilitaryGameEventLib.Instance.RemoveSelectedSoldier += OnRemoveSelectedSoldier;
        }

        public override void UnregisterEvents()
        {
            MilitaryGameEventLib.Instance.ShowBuildingInfo -= OnShowBuildingInfo;
            MilitaryGameEventLib.Instance.CloseInformationPanel -= OnCloseInformationPanel;
            MilitaryGameEventLib.Instance.SetDamageableObject -= OnSetDamageableObject;
            MilitaryGameEventLib.Instance.GetCurrentDamageableObject -= OnGetCurrentDamageableObject;
            MilitaryGameEventLib.Instance.AddSelectedSoldier -= OnAddSelectedSoldier;
            MilitaryGameEventLib.Instance.RemoveSelectedSoldier -= OnRemoveSelectedSoldier;
        }

        #region LISTENERS
        
        private void OnAddSelectedSoldier(Soldier.Soldier soldier)
        {
            _selectedSoldier.Add(soldier);
        }
        
        private void OnRemoveSelectedSoldier(Soldier.Soldier soldier)
        {
            _selectedSoldier.Remove(soldier);
        }

        private void OnShowBuildingInfo(BaseBuilding building)
        {
            _informationPanelController.ShowBuildingInfo(building);
        }

        private void OnCloseInformationPanel()
        {
            _informationPanelController.Close();
        }

        private IDamageable OnGetCurrentDamageableObject()
        {
            return _currenDamageableObject;
        }

        private void OnSetDamageableObject(IDamageable damageableObject)
        {
            if(_selectedSoldier.Count > 0)
                _currenDamageableObject = damageableObject;
        }

        #endregion
        

        public override void End()
        {
            base.End();
        
            _productMenuController.End();
            _informationPanelController.End();
            
            //BuildingFactory.Instance.End();
            //SoldierFactory.Instance.End();
        }
    }
}
