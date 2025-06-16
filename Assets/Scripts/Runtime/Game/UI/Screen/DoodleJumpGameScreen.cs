using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Game.UI.Screen
{
    public class DoodleJumpGameScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private TextMeshProUGUI _timeText;
        
        private DoodleGameData _data;
        
        public event Action OnBackPressed;
        
        [Inject]
        private void Construct(DoodleGameData data)
        {
            _data = data;
            
            _data.OnCoinsChanged += UpdateCoins;
            _data.OnTimeChanged += UpdateTime;
        }

        private void OnDestroy()
        {
            _data.OnCoinsChanged -= UpdateCoins;
            _data.OnTimeChanged -= UpdateTime;
        }

        private void UpdateCoins(int coins) => _coinsText.text = coins.ToString();

        private void UpdateTime(float time) => _timeText.text = Tools.Helper.FormatTime(time);

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }
    }
}