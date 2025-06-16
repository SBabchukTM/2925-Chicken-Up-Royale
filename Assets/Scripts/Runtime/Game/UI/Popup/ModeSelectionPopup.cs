using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Core.UI.Popup
{
    public class ModeSelectionPopup : BasePopup
    {
        [SerializeField] private Button _incubatorButton;
        [SerializeField] private Button _visitButton;
        [SerializeField] private Button _closeButton;
        
        public event Action OnIncubatorButtonPressed;
        public event Action OnVisitButtonPressed;
        public event Action OnCloseButtonPressed;

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            _incubatorButton.onClick.AddListener(() => OnIncubatorButtonPressed?.Invoke());
            _visitButton.onClick.AddListener(() => OnVisitButtonPressed?.Invoke());
            _closeButton.onClick.AddListener(() => OnCloseButtonPressed?.Invoke());
            return base.Show(data, cancellationToken);
        }
    }
}