using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class DailyBonusScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _parent;
        
        public event Action OnBackPressed;

        public void Initialize(List<DailyRewardDisplay> dailyRewardDisplayList)
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());

            foreach (var rewardDisplay in dailyRewardDisplayList)
            {
                rewardDisplay.transform.SetParent(_parent, false);
            }
        }
    }
}