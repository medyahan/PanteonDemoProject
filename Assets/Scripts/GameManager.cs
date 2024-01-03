using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private MilitaryGameManager _militaryGameManager;

    private void Start()
    {
        _militaryGameManager.Initialize();
    }

    private void OnDestroy()
    {
        _militaryGameManager.End();
    }
}
