using System.Collections;
using System.Collections.Generic;
using Core;
using MilitaryGame.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlotButton : BaseMonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _productIconImage;
    [SerializeField] private TMP_Text _productNameText;

    private BuildingData _buildingData;

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        _buildingData = (BuildingData) list[0];
        
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickProductButton);
        
        SetData(_buildingData);
    }

    private void SetData(BuildingData buildingData)
    {
        _productIconImage.sprite = buildingData.Icon;
        _productNameText.text = buildingData.Name;
    }
    
    private void OnClickProductButton()
    {
        BaseBuilding tempBaseBuilding = GridBuildingSystem.Current.TempBaseBuilding;
        if(tempBaseBuilding != null && !tempBaseBuilding.Placed)
            GridBuildingSystem.Current.ClearTempBuilding();
        
        GridBuildingSystem.Current.InitializeWithBuilding(_buildingData.Prefab);
    }
}
