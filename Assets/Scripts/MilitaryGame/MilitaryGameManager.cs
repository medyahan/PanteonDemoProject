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

    [Header("MILITARY GAME CONTROLLERS")] 
    [SerializeField] private GameBoardManager _gameBoardManager;
    
    #endregion // Variable Fields
    
    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        _productMenuController.Initialize();
        _informationPanelController.Initialize();
        
        _gameBoardManager.Initialize();
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
        base.End();
        
        _productMenuController.End();
        _informationPanelController.End();
        
        _gameBoardManager.End();
    }
}
