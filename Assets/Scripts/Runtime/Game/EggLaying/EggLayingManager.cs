using System;
using Runtime.Core.Audio;
using Runtime.Game.Care;
using Runtime.Game.EggLaying;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EggLayingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _layTimeLeftText;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Image _progressImage;
    
    private EggLayingService _eggLayingService;
    private ChickenCareService _chickenCareService;
    private IAudioService _audioService;

    [Inject]
    private void Construct(EggLayingService eggLayingService, ChickenCareService chickenCareService, IAudioService audioService)
    {
        _eggLayingService = eggLayingService;
        _chickenCareService = chickenCareService;
        _audioService = audioService;
    }

    private void Awake()
    {
        _claimButton.onClick.AddListener(ClaimEgg);
        _eggLayingService.UpdateEggTimeOffline();
        _claimButton.interactable = false;
        _progressImage.fillAmount = 0;
    }

    private void Update()
    {
        var chick = _chickenCareService.GetActiveChickenStatus();
        
        if (chick is not { ItemType: ItemType.Hen })
        {
            _layTimeLeftText.text = "--:--:--";
            return;
        }
        
        _eggLayingService.UpdateEggTimeOnline(Time.deltaTime);

        if (chick.ClaimableEgg)
        {
            _progressImage.fillAmount = 1;
            _claimButton.interactable = true;
            _layTimeLeftText.text = "CLAIM";
        }
        else
        {
            _progressImage.fillAmount = 1 - chick.LayTimeLeft * 1f / chick.TotalLayTime; 
            _claimButton.interactable = false;
            _layTimeLeftText.text = Helper.FormatTimeWithHours(chick.LayTimeLeft);
        }
    }

    private void ClaimEgg()
    {
        _eggLayingService.AddRandomEgg();
        var chick = _chickenCareService.GetActiveChickenStatus();
        chick.ClaimableEgg = false;
        int timeLeft = _eggLayingService.SelectLayEggTime();
        chick.LayTimeLeft = timeLeft;
        chick.TotalLayTime = timeLeft;
        _audioService.PlaySound(ConstAudio.EggCollectSound);
    }
}
