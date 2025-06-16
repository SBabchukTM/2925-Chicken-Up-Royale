using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SplashScreen : UiScreen
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TextMeshProUGUI _progressText;
        
        public override async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await SimulateLoadingWithStutter(cancellationToken);
            await base.HideAsync(cancellationToken);
        }
        
        private async UniTask SimulateLoadingWithStutter(CancellationToken cancellationToken)
        {
            float targetProgress = 0f;
            float currentProgress = 0f;

            _loadingSlider.value = 0f;
            UpdateProgressText(0f);

            while (currentProgress < 0.995f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                targetProgress = Mathf.Min(1f, currentProgress + Random.Range(0.05f, 0.15f));
                float stutterDelay = Random.Range(0.1f, 0.4f);

                float elapsed = 0f;
                float duration = stutterDelay;

                while (elapsed < duration)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    elapsed += Time.deltaTime;
                    currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime / duration * (targetProgress - currentProgress));
                    _loadingSlider.value = currentProgress;
                    UpdateProgressText(currentProgress);
                    
                    if(currentProgress >= 0.995f)
                        break;

                    await UniTask.Yield(cancellationToken);
                }
            }

            _loadingSlider.value = 1f;
            UpdateProgressText(1f);
        }

        private void UpdateProgressText(float progress)
        {
            int percent = Mathf.RoundToInt(progress * 100f);
            _progressText.text = $"Loading... {percent}%";
        }
    }
}