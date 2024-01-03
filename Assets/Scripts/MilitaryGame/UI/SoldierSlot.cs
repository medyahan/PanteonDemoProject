using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierSlot : BaseMonoBehaviour
{
    [SerializeField] private Button _produceButton;
    [SerializeField] private Image _soldierIconImage;
    [SerializeField] private TMP_Text _soldierNameText;
    [SerializeField] private TMP_Text _soldierAttackText;

    private SoldierData _soldierData;

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _soldierData = (SoldierData) list[0];
        
        _produceButton.onClick.RemoveAllListeners();
        _produceButton.onClick.AddListener(OnClickProduceButton);
        
        SetData(_soldierData);
    }

    private void SetData(SoldierData soldierData)
    {
        _soldierIconImage.sprite = soldierData.Icon;
        _soldierNameText.text = soldierData.Name;
        _soldierAttackText.text = "Att: " + soldierData.DamagePoint;
    }
    
    private void OnClickProduceButton()
    {
        
    }
}
    