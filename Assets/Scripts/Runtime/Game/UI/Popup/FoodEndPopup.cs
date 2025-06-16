using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class FoodEndPopup : BasePopup
    {
        [SerializeField] Button _button;
        
        public event Action OnClicked;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _button.onClick.AddListener(() => OnClicked?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}