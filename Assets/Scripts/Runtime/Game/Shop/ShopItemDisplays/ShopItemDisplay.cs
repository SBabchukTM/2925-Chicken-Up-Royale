using System;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class ShopItemDisplay : MonoBehaviour
{
    [FormerlySerializedAs("_button")] [SerializeField] protected Button Button;
    [SerializeField] private Image _image;
    [SerializeField] protected TextMeshProUGUI _priceText;
    
    public event Action<ShopItemDisplay> OnPurchased;

    private int _price;
    
    public int Price => _price;

    private void Awake()
    {
        Button.onClick.AddListener(InvokePurchased);
    }

    public void SetData(Sprite sprite, int price)
    {
        _price = price;
        
        _image.sprite = sprite;
        _priceText.text = price.ToString();
    }
    
    protected virtual void InvokePurchased()
    {
        OnPurchased?.Invoke(this);
    }
}
