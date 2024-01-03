using System.Collections;
using System.Collections.Generic;
using Core;
using MilitaryGame.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BuildingType = BuildingData.BuildingType;

public class InformationPanelController : BaseMonoBehaviour
{
    #region Variable Fields
    
    [Header("PANELS")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _productionPanel;

    [Header("INFO COMPONENTS")]
    [SerializeField] private Image _buildingIconImage;
    [SerializeField] private TMP_Text _buildingNameText;
    [SerializeField] private TMP_Text _buildingInfoText;

    [Header("SOLDIER SLOT")]
    [SerializeField] private SoldierSlot _soldierSlotPrefab;
    [SerializeField] private Transform _productionContentParent;
    [SerializeField] private List<SoldierData> _soldierDataList = new List<SoldierData>();
    
    private List<SoldierSlot> _soldierSlotList = new List<SoldierSlot>();
    private BaseBuilding _tempBuilding;

    #endregion // Variable Fields

    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        Close();
    }

    private void Open()
    {
        _mainPanel.SetActive(true);
    }
    
    public void Close()
    {
        _mainPanel.SetActive(false);
    }

    public void ShowBuildingInfo(BaseBuilding building)
    {
        Open();

        _tempBuilding = building;

        _buildingIconImage.sprite = building.BuildingData.Icon;
        _buildingInfoText.text = building.BuildingData.InfoString;
        _buildingNameText.text = building.BuildingData.Name;
        
        CheckProductive(building.BuildingData.IsProductive);
    }

    private void CheckProductive(bool isProductive)
    {
        _productionPanel.SetActive(isProductive);

        if (isProductive && _tempBuilding.BuildingData.Type == BuildingType.Barrack)
        {
            //CreateSoldierSlots();
        }
    }

    #region SOLDIER SLOT TRANSACTIONS
    
    private void CreateSoldierSlots()
    {
        ClearAllSoldierSlot();
        
        Barrack barrack = _tempBuilding as Barrack;
        
        for (int i = 0; i < _soldierDataList.Count; i++)
        {
            SoldierSlot soldierSlot = Instantiate(_soldierSlotPrefab, _productionContentParent);

            if (barrack is not null) 
                soldierSlot.Initialize(_soldierDataList[i], barrack.SoldierSpawnPoint);

            _soldierSlotList.Add(soldierSlot);
        }
    }
    
    private void ClearAllSoldierSlot()
    {
        foreach (SoldierSlot soldierSlot in _soldierSlotList)
        {
            if(soldierSlot == null)
                continue;
                
            soldierSlot.End();
            Destroy(soldierSlot.gameObject);
        }

        _soldierSlotList.Clear();
    }

    #endregion
}
