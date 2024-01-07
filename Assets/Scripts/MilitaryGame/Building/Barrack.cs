using System.Collections.Generic;
using MilitaryGame.Factory;
using MilitaryGame.GridBuilding;
using UnityEngine;
using UnityEngine.Tilemaps;
using SoldierType = SoldierData.SoldierType;

namespace MilitaryGame.Building
{
    public class Barrack : BaseBuilding
    {
        [Header("SOLDIER SPAWN")] 
        [SerializeField] private Transform _soldierSpawnPoint;

        /// <summary>
        /// Produces a soldier of the specified type, places it at the designated spawn point,
        /// initializes it, and adds it to the list of active soldiers.
        /// </summary>
        /// <param name="soldierType">Type of the soldier to produce.</param>
        public void ProduceSoldier(SoldierType soldierType)
        {
            Soldier.Soldier soldier = SoldierFactory.Instance.CreateSoldier(soldierType, Vector3.zero, Quaternion.identity);
            Tilemap mainTilemap = GridBuildingSystem.Instance.MainTilemap;
            Vector3Int soldierSpawnPos = mainTilemap.WorldToCell(_soldierSpawnPoint.position);
            soldier.transform.position = soldierSpawnPos;
            soldier.Initialize();
        }
    }
}