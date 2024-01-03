using Core;
using MilitaryGame.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MilitaryGame.UI.InformationPanel
{
    public class SoldierSlot : BaseMonoBehaviour
    {
        [SerializeField] private Button _produceButton;
        [SerializeField] private Image _soldierIconImage;
        [SerializeField] private TMP_Text _soldierNameText;
        [SerializeField] private TMP_Text _soldierAttackText;

        private SoldierData _soldierData;
        private Barrack _barrack;

        public override void Initialize(params object[] list)
        {
            base.Initialize(list);

            _soldierData = (SoldierData) list[0];
            _barrack = (Barrack) list[1];
        
            _produceButton.onClick.RemoveAllListeners();
            _produceButton.onClick.AddListener(OnClickProduceButton);
        
            SetData(_soldierData);
        }

        // Sets the UI data for displaying information about a soldier.
        private void SetData(SoldierData soldierData)
        {
            _soldierIconImage.sprite = soldierData.Icon;
            _soldierNameText.text = soldierData.Name;
            _soldierAttackText.text = "Att: " + soldierData.DamagePoint;
        }
    
        private void OnClickProduceButton()
        {
            _barrack.ProduceSoldier(_soldierData.Type);
        }
    }
}
    