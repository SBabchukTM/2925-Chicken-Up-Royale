using System;
using Runtime.Game.Care;
using Runtime.Game.Market;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CareManager : MonoBehaviour
{
    [SerializeField] private Image _happinessImg;
    [SerializeField] private Button _happinessButton;
    [SerializeField] private Image _hungerImage;
    [SerializeField] private Button _hungerButton;
    [SerializeField] private Image _cleanlinessImage;
    [SerializeField] private Button _cleanlinessButton;
    
    private ItemDataService _itemDataService;
    private ChickenCareService _chickenCareService;
    
    [Inject]
    private void Construct(ItemDataService itemDataService, ChickenCareService chickenCareService)
    {
        _itemDataService = itemDataService;
        _chickenCareService = chickenCareService;
    }
    
    private void Update()
    {
        var activeChick = _chickenCareService.GetActiveChickenStatus();
        if (activeChick == null)
        {
            _happinessButton.interactable = false;
            _hungerButton.interactable = false;
            _cleanlinessButton.interactable = false;
            return;
        }

        UpdateImage(_happinessImg, activeChick.Happiness);
        UpdateImage(_hungerImage, activeChick.Hunger);
        UpdateImage(_cleanlinessImage, activeChick.Cleanliness);
    }

    private void UpdateImage(Image slider, float value)
    {
        slider.fillAmount = value;
    }
}
