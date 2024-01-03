using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Soldier : BaseMonoBehaviour
{
    [SerializeField] private SoldierData _soldierData;
    [SerializeField] private HealthBar _healthBar;

    public SoldierData SoldierData => _soldierData;

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _healthBar.Initialize((float)_soldierData.HealthPoint);
    }

    public override void End()
    {
        
    }
}
