using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class MarketItemSelectPopup : BasePopup
    {
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _parent;
        
        public event Action OnClose;
        
        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _button.onClick.AddListener(() => OnClose?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetItems(List<MarketItemSelect> items)
        {
            foreach (var item in items)
                item.transform.SetParent(_parent, false);
        }
    }
}