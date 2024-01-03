using System.Collections;
using System.Collections.Generic;
using MilitaryGame.Building;
using MilitaryGame.Buildings;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string Name;
    public BuildingType Type;
    public bool IsProductive;
    public Sprite Icon;
    public int HealthPoint;
    public Building Prefab;
    [TextArea(4, 10)] public string InfoString;
    
    public enum BuildingType
    {
        Barrack,
        PowerPlant,
    }
}
