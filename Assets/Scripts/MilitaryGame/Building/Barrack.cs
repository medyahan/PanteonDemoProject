using System.Collections.Generic;
using UnityEngine;
using SoldierType = SoldierData.SoldierType;

namespace MilitaryGame.Building
{
    public class Barrack : BaseBuilding
    {
        [Header("SOLDIER SPAWN")] 
        [SerializeField] private Transform _soldierSpawnPoint;

        private List<Soldier> _soldierList = new List<Soldier>();

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);
        }

        public override void End()
        {
            base.End();
            
            foreach (Soldier soldier in _soldierList)
            {
                soldier.End();
                SoldierFactory.Instance.DestroySoldier(soldier);
            }
            _soldierList.Clear();
        }

        public void ProduceSoldier(SoldierType soldierType)
        {
            Soldier soldier = SoldierFactory.Instance.CreateSoldier(soldierType, Vector3.zero, Quaternion.identity);
            soldier.transform.position = _soldierSpawnPoint.position;
            soldier.Initialize();
            
            _soldierList.Add(soldier);
        }
    }
}