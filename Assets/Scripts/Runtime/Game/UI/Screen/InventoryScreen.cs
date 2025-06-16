using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class InventoryScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _bonusItemsParent;
        [SerializeField] private RectTransform _eggsParent;
        [SerializeField] private RectTransform _chickensParent;
        [SerializeField] private RectTransform _hensParent;
        
        public event Action OnBackPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        public void SetBonusItems(List<BonusItemDisplay> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_bonusItemsParent, false);
        }
        
        public void SetEggItems(List<BaseItemDisplay> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_eggsParent, false);
        }
        
        public void SetChickenItems(List<ChickenItemDisplay> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_chickensParent, false);
        }
        
        public void SetHenItems(List<ChickenItemDisplay> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_hensParent, false);
        }
    }
}