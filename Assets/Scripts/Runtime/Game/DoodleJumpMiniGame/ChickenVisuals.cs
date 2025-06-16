using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Care;
using Runtime.Game.Market;
using UnityEngine;
using Zenject;

public class ChickenVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private ItemDataService _itemDataService;
    private ChickenCareService _chickenCareService;
    
    [Inject]
    private void Construct(ItemDataService itemDataService, ChickenCareService chickenCareService)
    {
        _itemDataService = itemDataService;
        _chickenCareService = chickenCareService;
    }

    private void Start()
    {
        var activeChick = _chickenCareService.GetActiveChickenStatus();
        if (activeChick == null)
        {
            _spriteRenderer.gameObject.SetActive(false);
            return;
        }

        _spriteRenderer.sprite = _itemDataService.GetItemSprite(activeChick.ItemType, activeChick.Id);
    }

    public void UpdateVisuals(float moveDir)
    {
        if(moveDir == 0)
            return;
        
        _spriteRenderer.flipX = moveDir > 0;
    }
}
