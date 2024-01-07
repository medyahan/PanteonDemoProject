using System;
using Interfaces.MilitaryGame;
using MilitaryGame.Building;

namespace MilitaryGame
{
    public class MilitaryGameEventLib : Singleton<MilitaryGameEventLib>
    {
        public Action<BaseBuilding> ShowBuildingInfo;
        public Action CloseInformationPanel;
        public Action<IDamageable> SetDamageableObject;
        public Func<IDamageable> GetCurrentDamageableObject;

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
        }
    }
}
