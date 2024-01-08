using System;
using Interface;
using MilitaryGame.Building;

namespace MilitaryGame
{
    public class MilitaryGameEventLib : Singleton<MilitaryGameEventLib>
    {
        public Action<BaseBuilding> ShowBuildingInfo;
        public Action CloseInformationPanel;
        public Action<IDamageable> SetDamageableObject;
        public Func<IDamageable> GetCurrentDamageableObject;
        public Action<Soldier.Soldier> AddSelectedSoldier;
        public Action<Soldier.Soldier> RemoveSelectedSoldier;

        protected override void Awake()
        {
            DestroyOnLoad = true;
            base.Awake();
        }

        public void OnDestroy()
        {
            ShowBuildingInfo = null;
            CloseInformationPanel = null;
            SetDamageableObject = null;
            GetCurrentDamageableObject = null;
            AddSelectedSoldier = null;
            RemoveSelectedSoldier = null;
        }
    }
}
