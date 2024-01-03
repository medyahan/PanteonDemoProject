using System.Collections.Generic;
using UnityEngine;
using SoldierType = SoldierData.SoldierType;

namespace MilitaryGame.Building
{
    public class Barrack : BaseBuilding
    {
        [Header("SOLDIER SPAWN")] 
        [SerializeField] private Transform _soldierSpawnPoint;
        public Transform SoldierSpawnPoint => _soldierSpawnPoint;

        [Header("SOLDIER PREFABS")] 
        [SerializeField] private Soldier _ordinaryPref;
        [SerializeField] private Soldier _helmetPref;
        [SerializeField] private Soldier _generalPref;

        private Dictionary<SoldierType, Soldier> _soldiers = new Dictionary<SoldierData.SoldierType, Soldier>();

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
            
            // _soldiers.Add(SoldierType.Ordinary, _ordinaryPref);
            // _soldiers.Add(SoldierType.Helmet, _helmetPref);
            // _soldiers.Add(SoldierType.General, _helmetPref);
        }

        public override void RegisterEvents()
        {
            
        }

        public override void UnregisterEvents()
        {
            
        }
    }
}