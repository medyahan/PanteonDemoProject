using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class MilitaryGameManager : BaseMonoBehaviour
{
    #region Variable Fields
    
    [Header("UI CONTROLLERS")]
    [SerializeField] private ProductMenuController _productMenuController;
    [SerializeField] private InformationPanelController _informationPanelController;

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
    }

    public override void UnregisterEvents()
    {
        MilitaryGameEventLib.Instance.ShowBuildingInfo -= _informationPanelController.ShowBuildingInfo;
        MilitaryGameEventLib.Instance.CloseInformationPanel -= _informationPanelController.Close;
    }

    public override void End()
    {
        
        _productMenuController.End();
        _informationPanelController.End();
        base.End();
    }
}
