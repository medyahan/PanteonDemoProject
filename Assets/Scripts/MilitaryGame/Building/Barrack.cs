using MilitaryGame.Factory;
using MilitaryGame.GridBuilding;
using UnityEngine;
using UnityEngine.Tilemaps;
using SoldierType = Data.MilitaryGame.SoldierData.SoldierType;

namespace MilitaryGame.Building
{
    public class Barrack : BaseBuilding
    {
        [Header("SOLDIER SPAWN")] 
        [SerializeField] private Transform _soldierSpawnPoint;

        /// <summary>
        /// Produces a soldier of the specified type, placing it at the designated spawn point.
        /// </summary>
        /// <param name="soldierType">The type of soldier to produce.</param>
        public void ProduceSoldier(SoldierType soldierType)
        {
            Soldier.Soldier soldier = SoldierFactory.Instance.CreateSoldier(soldierType, Vector3.zero, Quaternion.identity);
     
            Vector3Int soldierSpawnPos = GridBuildingSystem.Instance.MainTilemap.WorldToCell(_soldierSpawnPoint.position);
            soldier.transform.position = soldierSpawnPos;
            soldier.Initialize();
        }
    }
}