using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _howToPlayButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _dailyButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _achievementsButton;
        [SerializeField] private Button _marketButton;
        
        public event Action OnHowToPlayPressed;
        public event Action OnSettingsPressed;
        public event Action OnProfilePressed;
        public event Action OnShopPressed;
        public event Action OnDailyPressed;
        public event Action OnPlayPressed; 
        public event Action OnAchievementsPressed;
        public event Action OnMarketPressed;

        public void Initialize()
        {
            _howToPlayButton.onClick.AddListener(() => OnHowToPlayPressed?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _profileButton.onClick.AddListener(() => OnProfilePressed?.Invoke());
            _shopButton.onClick.AddListener(() => OnShopPressed?.Invoke());
            _dailyButton.onClick.AddListener(() => OnDailyPressed?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _achievementsButton.onClick.AddListener(() => OnAchievementsPressed?.Invoke());
            _marketButton.onClick.AddListener(() => OnMarketPressed?.Invoke());
        }
    }
}