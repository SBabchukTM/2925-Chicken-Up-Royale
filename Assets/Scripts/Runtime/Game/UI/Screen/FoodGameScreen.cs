using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class FoodGameScreen : UiScreen
    {
        private const float TakeAnimTime = 0.2f;
        private const float PutAnimTime = 0.15f;
        private const float HitAnimTime = 0.1f;
        private const float AwayAnimTime = 0.17f;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Button _hammerButton;
        [SerializeField] private Image _hammerItemImage;
        [SerializeField] private Image _hammerImage;
        
        public event Action OnBackPressed;
        public event Action OnHammerPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _hammerButton.onClick.AddListener(() =>
            {
                OnHammerPressed?.Invoke();
                TakeHammer();
            });
        }

        public void UpdateProgress(int progress, int target)
        {
            _progressText.text = $"{progress}/{target}";
        }

        private void TakeHammer()
        {
            _hammerItemImage.DOFade(0, TakeAnimTime).SetLink(gameObject);
        }

        private void PutHammer()
        {
            _hammerItemImage.DOFade(1, PutAnimTime).SetLink(gameObject);
        }

        public void PlayHammerAnim(Vector3 position)
        {
            _hammerImage.transform.position = position;
            Sequence sequence = DOTween.Sequence(_hammerImage.gameObject);
            
            sequence.Append(_hammerImage.DOFade(1, TakeAnimTime));
            sequence.Append(_hammerImage.transform.DOLocalRotate(new Vector3(0, 0, 90), HitAnimTime));
            sequence.Append(_hammerImage.transform.DOLocalRotate(new Vector3(0, 0, 45), AwayAnimTime));
            sequence.Append(_hammerImage.DOFade(0, PutAnimTime));

            PutHammer();
        }
    }
}