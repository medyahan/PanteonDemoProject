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
    
    [Header("BUTTON")]
    [SerializeField] private Button _headerMenuButton;
    
    [Header("MENU VALUES")]
    [SerializeField] private float _openCloseAnimateDuration;
    [SerializeField] private float _closedMenuPositionY;

    private List<BuildingSlotButton> _buildingSlotButtonList = new List<BuildingSlotButton>();
    private bool _isMenuOpen;

    #endregion // Variable Fields
    
    public override void Initialize(params object[] list)
    {
        base.Initialize(list);
        
        //_headerMenuButton.onClick.RemoveAllListeners();
        //_headerMenuButton.onClick.AddListener(OnClickProductMenuButton);
        //ResetMenuPositionAsClose();
        
        CreateProductSlotButtons();
    }

    private void ResetMenuPositionAsClose()
    {
        Vector3 position = transform.position;
        position.y = _closedMenuPositionY;
        transform.position = position;
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

    private void OnClickProductMenuButton()
    {
        if (_isMenuOpen)
        {
            transform.DOMoveY(_closedMenuPositionY, _openCloseAnimateDuration);
        }
        else
        {
            transform.DOMoveY(0, _openCloseAnimateDuration);
        }

        _isMenuOpen = !_isMenuOpen;
    }
}
