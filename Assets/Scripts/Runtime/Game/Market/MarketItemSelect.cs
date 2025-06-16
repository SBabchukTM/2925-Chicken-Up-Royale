using System;
using Runtime.Game.Services.UserData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItemSelect : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _amountText;
    
    private ItemData _itemData;

    public event Action<ItemData> OnSelect;
    
    public void Initialize(ItemData itemData, Sprite icon, int amount)
    {
        _itemData = itemData;
        _image.sprite = icon;
        _amountText.text = amount.ToString();
        _button.onClick.AddListener(() => OnSelect?.Invoke(_itemData));
    }
}
