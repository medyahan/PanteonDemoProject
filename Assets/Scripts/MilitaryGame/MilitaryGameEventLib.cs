using System;
using MilitaryGame.Building;

namespace MilitaryGame
{
    public class MilitaryGameEventLib : Singleton<MilitaryGameEventLib>
    {
        public Action<BaseBuilding> ShowBuildingInfo;
        public Action CloseInformationPanel;
        public Action<BaseBuilding> SetSelectedBuildingForAttack;
        public Func<BaseBuilding> GetSelectedBuildingForAttack;

        protected override void Awake()
        {
            DestroyOnLoad = true;
            base.Awake();
        }

        public void OnDestroy()
        {
            ShowBuildingInfo = null;
            CloseInformationPanel = null;
            SetSelectedBuildingForAttack = null;
            GetSelectedBuildingForAttack = null;
        }
    }
}
