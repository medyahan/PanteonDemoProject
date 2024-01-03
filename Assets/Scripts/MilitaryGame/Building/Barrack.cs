using System.Collections.Generic;
using MilitaryGame.Factory;
using UnityEngine;
using SoldierType = SoldierData.SoldierType;

namespace MilitaryGame.Building
{
    public class Barrack : BaseBuilding
    {
        [Header("SOLDIER SPAWN")] 
        [SerializeField] private Transform _soldierSpawnPoint;

        private List<Soldier.Soldier> _soldierList = new List<Soldier.Soldier>();
        
        /// <summary>
        /// Produces a soldier of the specified type, places it at the designated spawn point,
        /// initializes it, and adds it to the list of active soldiers.
        /// </summary>
        /// <param name="soldierType">Type of the soldier to produce.</param>
        public void ProduceSoldier(SoldierType soldierType)
        {
            Soldier.Soldier soldier = SoldierFactory.Instance.CreateSoldier(soldierType, Vector3.zero, Quaternion.identity);
            soldier.transform.position = _soldierSpawnPoint.position;
            soldier.Initialize();
            
            _soldierList.Add(soldier);
        }
        
        public override void End()
        {
            base.End();
            
            foreach (Soldier.Soldier soldier in _soldierList)
            {
                if(soldier == null)
                    continue;
                
                soldier.End();
                Destroy(soldier.gameObject);
            }
            _soldierList.Clear();
        }
    }
}