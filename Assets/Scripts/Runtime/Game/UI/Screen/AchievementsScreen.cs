using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class AchievementsScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _parent;
        
        public event Action OnBackPressed;

        public void Initialize(List<AchievementDisplay> achievementDisplayList)
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());

            foreach (var display in achievementDisplayList)
            {
                display.transform.SetParent(_parent, false);
            }
        }
    }
}