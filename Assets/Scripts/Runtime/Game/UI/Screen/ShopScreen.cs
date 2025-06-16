using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class ShopScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private ShopModeButton[] _shopModeButtons;
        
        public event Action OnBackPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());

            foreach (var button in _shopModeButtons)
            {
                button.OnClicked += UpdateShopMode;
            }
        }

        private void UpdateShopMode(ShopModeButton obj)
        {
            for (int i = 0; i < _shopModeButtons.Length; i++)
            {
                var button = _shopModeButtons[i];
                button.EnableView(obj == button);
            }
        }
    }
}