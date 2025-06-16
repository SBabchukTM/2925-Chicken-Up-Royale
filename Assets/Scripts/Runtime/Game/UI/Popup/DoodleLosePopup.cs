using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class DoodleLosePopup : BasePopup
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _bestTimeText;
        
        public event Action OnPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _button.onClick.AddListener(() => OnPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }

        public void SetBestTime(float time)
        {
            _bestTimeText.text = Helper.FormatTime(time);
        }
    }
}