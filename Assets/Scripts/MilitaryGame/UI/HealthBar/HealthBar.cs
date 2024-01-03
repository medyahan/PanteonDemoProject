using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : BaseMonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _healtValueText;
    [SerializeField] private float _fillDuration;

    private float _maxValue;
    
    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _maxValue = (float) list[0];
        
        SetHealthBar(_maxValue);
    }

    public void SetHealthBar(float currentValue)
    {
        _fillImage.DOFillAmount(currentValue / _maxValue, _fillDuration);
        _healtValueText.text = ((int)currentValue).ToString();
    }
}