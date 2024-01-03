using System;
using System.Collections;
using System.Collections.Generic;
using MilitaryGame.Building;
using MilitaryGame.Buildings;
using UnityEngine;

public class MilitaryGameEventLib : Singleton<MilitaryGameEventLib>
{
    public Action<Building> ShowBuildingInfo;
    public Action CloseInformationPanel;

    protected override void Awake()
    {
        DestroyOnLoad = true;
        base.Awake();
    }

    private void OnDestroy()
    {
        ShowBuildingInfo = null;
        CloseInformationPanel = null;
    }
}
