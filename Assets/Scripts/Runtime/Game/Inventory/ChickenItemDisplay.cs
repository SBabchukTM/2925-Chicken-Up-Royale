using System;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using UnityEngine.UI;

public class ChickenItemDisplay : BaseItemDisplay
{
    [SerializeField] private Button _button;
    
    public event Action<ItemData> OnSelected;

    public void InitializeButton(ItemData itemData)
    {
        _button.onClick.AddListener(() => OnSelected?.Invoke(itemData));
    }
}
