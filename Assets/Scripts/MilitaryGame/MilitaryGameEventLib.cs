using System;
using MilitaryGame.Building;

namespace MilitaryGame
{
    public class MilitaryGameEventLib : Singleton<MilitaryGameEventLib>
    {
        public Action<BaseBuilding> ShowBuildingInfo;
        public Action CloseInformationPanel;

        protected override void Awake()
        {
            DestroyOnLoad = true;
            base.Awake();
        }

        private void OnDestroy()
        {
            ShowBuildingInfo = null;
            CloseInformationPanel = null;
        }
    }
}
