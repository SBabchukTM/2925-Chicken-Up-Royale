using System;
using UnityEngine;
using UnityEngine.UI;

public class BonusItemDisplay : BaseItemDisplay
{
    [SerializeField] private Button _button;

    public event Action<int> OnUsed;

    private int _itemID;
    public int ItemID => _itemID;
    
    public void InitializeButton(int itemId)
    {
        _itemID = itemId;
        _button.onClick.AddListener(() => OnUsed?.Invoke(itemId));
    }
}
