using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Services;
using TMPro;
using UnityEngine;
using Zenject;

public class BalanceDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;

    private UserInventoryService _userInventoryService;
    
    [Inject]
    private void Construct(UserInventoryService userInventoryService)
    {
        _userInventoryService = userInventoryService;

        UpdateBalance(_userInventoryService.GetBalance());
        _userInventoryService.OnBalanceChanged += UpdateBalance;
    }

    private void OnDestroy()
    {
        _userInventoryService.OnBalanceChanged -= UpdateBalance;
    }

    private void UpdateBalance(int obj)
    {
        _balanceText.text = obj.ToString();
    }
}
