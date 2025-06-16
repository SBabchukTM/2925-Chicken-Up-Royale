using System;
using Runtime.Game.Care;
using Runtime.Game.Market;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.UI.Screen
{
    public class BathGameScreen : UiScreen
    {
        [SerializeField] private Image _chickenImage;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Image _itemImage;

        [SerializeField] private Button _waterButton;
        [SerializeField] private Button _soapButton;
        [SerializeField] private Button _brushButton;

        [SerializeField] private Sprite _waterSprite;
        [SerializeField] private Sprite _soapSprite;
        [SerializeField] private Sprite _brushSprite;

        [SerializeField] private Image _waterImage;
        [SerializeField] private Image _dirtImage;
        [SerializeField] private Image _soapImage;
        
        public event Action OnBackPressed;
        public event Action OnWaterPressed;
        public event Action OnSoapPressed;
        public event Action OnBrushPressed;
        
        private ItemDataService _itemDataService;
        private ChickenCareService _chickenCareService;
    
        [Inject]
        private void Construct(ItemDataService itemDataService, ChickenCareService chickenCareService)
        {
            _itemDataService = itemDataService;
            _chickenCareService = chickenCareService;
        }

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            
            _waterButton.onClick.AddListener(() =>
            {
                _itemImage.sprite = _waterSprite;
                OnWaterPressed?.Invoke();
            });
            _soapButton.onClick.AddListener(() =>
            {
                _itemImage.sprite = _soapSprite;
                OnSoapPressed?.Invoke();
            });
            _brushButton.onClick.AddListener(() =>
            {
                _itemImage.sprite = _brushSprite;
                OnBrushPressed?.Invoke();
            });

            SetChickenSkin();
        }

        private void SetChickenSkin()
        {
            var activeChick = _chickenCareService.GetActiveChickenStatus();
            if (activeChick == null)
            {
                _chickenImage.gameObject.SetActive(false);
                return;
            }

            _chickenImage.sprite = _itemDataService.GetItemSprite(activeChick.ItemType, activeChick.Id);
        }
        
        public void UpdateProgress(WashingItemType itemType, float progress)
        {
            UpdateItemSprite(itemType);
            UpdateProgress(progress);
            UpdateVisuals(itemType, progress);
        }

        private void UpdateProgress(float progress)
        {
            if (progress >= 1f)
            {
                _progressSlider.gameObject.SetActive(false);
                _itemImage.gameObject.SetActive(false);
            }
            else
            {
                _progressSlider.gameObject.SetActive(true);
                _itemImage.gameObject.SetActive(true);
                _progressSlider.value = progress;
            }
        }

        private void UpdateItemSprite(WashingItemType itemType)
        {
            switch (itemType)
            {
                case WashingItemType.Water:
                    _itemImage.sprite = _waterSprite;
                    break;
                case WashingItemType.Soap:
                    _itemImage.sprite = _soapSprite;
                    break;
                case WashingItemType.Brush:
                    _itemImage.sprite = _brushSprite;
                    break;
            }
        }

        private void UpdateVisuals(WashingItemType itemType, float progress)
        {
            switch (itemType)
            {
                case WashingItemType.Water:
                    UpdateAlpha(_waterImage, 0, 0.2f, progress);
                    UpdateAlpha(_dirtImage, 0.5f, 0.8f, 1 - progress);
                    break;
                case WashingItemType.Soap:
                    UpdateAlpha(_soapImage, 0, 0.8f, progress);
                    UpdateAlpha(_waterImage, 0, 0.2f, 1 - progress);
                    break;
                case WashingItemType.Brush:
                    UpdateAlpha(_dirtImage, 0, 0.5f, 1 - progress);
                    UpdateAlpha(_soapImage, 0, 0.8f, 1 - progress);
                    break;
            }
        }

        private void UpdateAlpha(Image image, float min, float max, float progress)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(min, max, progress);
            image.color = color;
        }
    }
}