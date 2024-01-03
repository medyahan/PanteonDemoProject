using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierData", menuName = "Data/SoldierData")]
public class SoldierData : ScriptableObject
{
    public string Name;
    public SoldierType Type;
    public Sprite Icon;
    public int HealthPoint;
    public int DamagePoint;
    
    public enum SoldierType
    {
        Ordinary,
        General,
        Helmet
    }
}
