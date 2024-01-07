using Core;
using Interfaces.MilitaryGame;
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

        private IDamageable _currenDamageableObject;

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
            MilitaryGameEventLib.Instance.SetDamageableObject += OnSetDamageableObject;
            MilitaryGameEventLib.Instance.GetCurrentDamageableObject += OnGetCurrentDamageableObject;
        }

        public override void UnregisterEvents()
        {
            MilitaryGameEventLib.Instance.ShowBuildingInfo -= _informationPanelController.ShowBuildingInfo;
            MilitaryGameEventLib.Instance.CloseInformationPanel -= _informationPanelController.Close;
            MilitaryGameEventLib.Instance.SetDamageableObject -= OnSetDamageableObject;
            MilitaryGameEventLib.Instance.GetCurrentDamageableObject -= OnGetCurrentDamageableObject;

        }

        private IDamageable OnGetCurrentDamageableObject()
        {
            return _currenDamageableObject;
        }

        private void OnSetDamageableObject(IDamageable damageableObject)
        {
            _currenDamageableObject = damageableObject;
        }

        public override void End()
        {
            base.End();
        
            _productMenuController.End();
            _informationPanelController.End();
        }
    }
}
