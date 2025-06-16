using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaItemButton : ShopItemDisplay
{
    [SerializeField] private Image _checkImage;
    
    private AreaType _areaType;
    public AreaType AreaType => _areaType;
    
    public void Initialize(bool purchased, AreaType areaType)
    {
        _areaType = areaType;
        if (purchased)
        {
            Button.interactable = false;
            _checkImage.gameObject.SetActive(true);
        }
    }
}
