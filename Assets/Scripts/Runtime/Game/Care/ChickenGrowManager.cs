using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Care;
using Runtime.Game.DailyRewards;
using Runtime.Game.Market;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChickenGrowManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _timerParent;
    [SerializeField] private GameObject _errorParent;
    
    private ChickenGrowService _chickenGrowService;
    private ChickenCareService _chickenCareService;
    private UserLoginService _userLoginService;
    private ItemDataService _itemDataService;

    [Inject]
    private void Construct(ChickenGrowService chickenGrowService, 
        ChickenCareService chickenCareService, UserLoginService userLoginService, ItemDataService itemDataService)
    {   
        _chickenGrowService = chickenGrowService;
        _chickenCareService = chickenCareService;
        _userLoginService = userLoginService;
        _itemDataService = itemDataService;
    }

    private void Awake()
    {
        _chickenGrowService.UpdateOfflineGrowTime();
    }

    private void Start()
    {
        _userLoginService.RecordChickenVisitTime();
        
        var chickenStatus = _chickenCareService.GetActiveChickenStatus();
        
        if (chickenStatus == null)
            _image.gameObject.SetActive(false);
        else
            _image.sprite = _itemDataService.GetItemSprite(chickenStatus.ItemType, chickenStatus.Id);
    }

    private void Update()
    {
        var chickenStatus = _chickenCareService.GetActiveChickenStatus();
        if (chickenStatus == null || chickenStatus.ItemType == ItemType.Hen)
        {
            _errorParent.SetActive(false);
            _timerParent.SetActive(false);
            _timerParent.transform.parent.gameObject.SetActive(false);
            return;
        }

        bool success = _chickenGrowService.UpdateOnlineGrowTime(Time.deltaTime);
        
        if (success)
        {
            _errorParent.SetActive(false);
            _timerParent.SetActive(true);
            _timeText.text = Helper.FormatTimeWithHours(chickenStatus.GrowTimeLeft);
        }
        else
        {
            _errorParent.SetActive(true);
            _timerParent.SetActive(false);
        }

        if (chickenStatus.ItemType == ItemType.Hen)
        {
            _timerParent.transform.parent.gameObject.SetActive(false);
            _image.sprite = _itemDataService.GetItemSprite(chickenStatus.ItemType, chickenStatus.Id);
        }
    }
}
