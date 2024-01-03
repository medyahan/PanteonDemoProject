using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoldierType = SoldierData.SoldierType;

public class SoldierFactory : Singleton<SoldierFactory>
{
    [Header("SOLDIER COUNT")]
    [SerializeField] private int _soldierCount;

    [Header("SOLDIER DATA LIST")]
    [SerializeField] private List<SoldierData> _soldierDataList = new List<SoldierData>();

    private Dictionary<SoldierType, ObjectPool<Soldier>> _soldierPoolList = new Dictionary<SoldierData.SoldierType, ObjectPool<Soldier>>();

    public void Initialize()
    {
        foreach (SoldierData soldierData in _soldierDataList)
        {
            _soldierPoolList.Add(soldierData.Type, new ObjectPool<Soldier>(transform, soldierData.Prefab, _soldierCount));
        }
    }
    
    public Soldier CreateSoldier(SoldierType soldierType, Vector3 position, Quaternion rotation)
    {
        return _soldierPoolList[soldierType].GetObject(position, rotation);
    }

    public void DestroySoldier(Soldier soldier)
    {
        _soldierPoolList[soldier.SoldierData.Type].ReturnObject(soldier);
    }
}
