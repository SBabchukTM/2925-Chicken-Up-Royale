using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class CareScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _dailyButton;
        [SerializeField] private Button _happyButton;
        [SerializeField] private Button _foodButton;
        [SerializeField] private Button _bathButton;
        
        
        public event Action OnBackPressed;
        public event Action OnInventoryPressed;
        public event Action OnShopPressed;
        public event Action OnDailyPressed;
        public event Action OnHappyPressed;
        public event Action OnFoodPressed;
        public event Action OnBathPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _inventoryButton.onClick.AddListener(() => OnInventoryPressed?.Invoke());
            _shopButton.onClick.AddListener(() => OnShopPressed?.Invoke());
            _dailyButton.onClick.AddListener(() => OnDailyPressed?.Invoke());
            _happyButton.onClick.AddListener(() => OnHappyPressed?.Invoke());
            _foodButton.onClick.AddListener(() => OnFoodPressed?.Invoke());
            _bathButton.onClick.AddListener(() => OnBathPressed?.Invoke());
        }
    }
}