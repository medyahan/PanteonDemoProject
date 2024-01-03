using System.Collections;
using System.Collections.Generic;
using MilitaryGame.Building;
using UnityEngine;
using BuildingType = BuildingData.BuildingType;

public class BuildingFactory : Singleton<BuildingFactory>
{
    [Header("BUILDING COUNT")]
    [SerializeField] private int _buildingCount;

    [Header("BUILDING DATA LIST")]
    [SerializeField] private List<BuildingData> _buildingDataList = new List<BuildingData>();

    private Dictionary<BuildingType, ObjectPool<BaseBuilding>> _buildingPoolList = new Dictionary<BuildingType, ObjectPool<BaseBuilding>>();

    public void Initialize()
    {
        foreach (BuildingData buildingData in _buildingDataList)
        {
            _buildingPoolList.Add(buildingData.Type, new ObjectPool<BaseBuilding>(transform, buildingData.Prefab, _buildingCount));
        }
    }

    public BaseBuilding CreateBuilding(BuildingType buildingType, Vector3 position, Quaternion rotation)
    {
        return _buildingPoolList[buildingType].GetObject(position, rotation);
    }

    public void DestroyBuilding(BaseBuilding building)
    {
        _buildingPoolList[building.BuildingData.Type].ReturnObject(building);
    }
}
