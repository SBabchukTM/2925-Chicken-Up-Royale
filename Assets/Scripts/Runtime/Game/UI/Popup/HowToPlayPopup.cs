using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class HowToPlayPopup : BasePopup
    {
        [SerializeField] private Button _backButton;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _backButton.onClick.AddListener(DestroyPopup);
            return base.Show(data, cancellationToken);
        }
    }
}