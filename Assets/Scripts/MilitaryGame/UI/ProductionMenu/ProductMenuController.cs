using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProductMenuController : BaseMonoBehaviour
{
    #region Variable Fields
    
    [SerializeField] private List<BuildingData> _buildingDataList;
    
    [Header("BUILDING SLOT")]
    [SerializeField] private BuildingSlotButton _buildingSlotButtonPref;
    [SerializeField] private Transform _contentParent;

    private List<BuildingSlotButton> _buildingSlotButtonList = new List<BuildingSlotButton>();
    private bool _isMenuOpen;

    #endregion // Variable Fields
    
    public override void Initialize(params object[] list)
    {
        base.Initialize(list);

        CreateProductSlotButtons();
    }
    
    private void CreateProductSlotButtons()
    {
        for (int i = 0; i < _buildingDataList.Count; i++)
        {
            BuildingSlotButton buildingSlotButton = Instantiate(_buildingSlotButtonPref, _contentParent);

            buildingSlotButton.Initialize(_buildingDataList[i]);
            
            _buildingSlotButtonList.Add(buildingSlotButton);
        }
    }

    public override void End()
    {
        base.End();
        
        foreach (BuildingSlotButton slotButton in _buildingSlotButtonList)
        {
            slotButton.End();
            Destroy(slotButton.gameObject);
        }
        _buildingSlotButtonList.Clear();
    }
}
