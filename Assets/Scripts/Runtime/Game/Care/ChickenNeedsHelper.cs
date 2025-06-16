using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Game.Care;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChickenNeedsHelper : MonoBehaviour
{
    private const float NormalThreshold = 0.9f;
    
    [SerializeField] private Image _helpImage;
    [SerializeField] private CanvasGroup _timerImage;
    [SerializeField] private Sprite _sadHelp;
    [SerializeField] private Sprite _hungryHelp;
    [SerializeField] private Sprite _dirtyHelp;
    [SerializeField] private Sprite _greatHelp;

    private ChickenCareService _chickenCareService;
    
    private ChickenStatus _chickenStatus;
    
    [Inject]
    private void Construct(ChickenCareService chickenCareService)
    {
        _chickenCareService = chickenCareService;
    }
    
    private void Awake()
    {
        _chickenStatus = _chickenCareService.GetActiveChickenStatus();
        if (_chickenStatus == null)
        {
            _helpImage.gameObject.SetActive(false);
            return;
        }
        
        StartCoroutine(ShowNeeds());
    }

    private IEnumerator ShowNeeds()
    {
        const float fadeDuration = 0.5f;
        const float displayDuration = 2f;
        const float loopDuration = 2f;
        const float timerDuration = 3.5f;

        var waitFade = new WaitForSeconds(fadeDuration);
        var waitDisplay = new WaitForSeconds(displayDuration);
        var waitLoop = new WaitForSeconds(loopDuration);
        var waitTimer = new WaitForSeconds(timerDuration);

        var image = _helpImage;
        var color = image.color;
        color.a = 0;
        image.color = color;
        image.gameObject.SetActive(true);

        while (true)
        {
            float happiness = _chickenStatus.Happiness;
            float hunger = _chickenStatus.Hunger;
            float cleanliness = _chickenStatus.Cleanliness;

            var unmetNeeds = new List<Sprite>();

            if (hunger < NormalThreshold)
                unmetNeeds.Add(_hungryHelp);
            if (happiness < NormalThreshold)
                unmetNeeds.Add(_sadHelp);
            if (cleanliness < NormalThreshold)
                unmetNeeds.Add(_dirtyHelp);

            if (unmetNeeds.Count == 0)
            {
                unmetNeeds.Add(_greatHelp);
            }

            foreach (var sprite in unmetNeeds)
            {
                image.sprite = sprite;
                image.DOFade(1f, fadeDuration);
                yield return waitFade;
                
                yield return waitDisplay;
                
                image.DOFade(0f, fadeDuration);
                yield return waitFade;
            }

            _timerImage.DOFade(1f, fadeDuration);
            yield return waitFade;
            
            yield return waitTimer;
            
            _timerImage.DOFade(0, fadeDuration);
            yield return waitFade;

            yield return waitLoop;
        }
    }
}
