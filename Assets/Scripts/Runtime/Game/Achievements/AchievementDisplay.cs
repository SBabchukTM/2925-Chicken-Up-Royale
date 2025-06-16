using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Image _claimedImage;

    private AchievementData _achievementData;
    
    [Inject] private IAudioService _audioService;
    [Inject] private UserInventoryService _userInventoryService;
    
    public void Initialize(AchievementData data)
    {
        _achievementData = data;
        
        _nameText.text = data.Name;
        _descText.text = data.Description;
        _rewardText.text = data.Reward.ToString();
        
        if(data.Unlocked && !data.Claimed)
            _claimButton.interactable = true;
        
        _claimedImage.gameObject.SetActive(data.Claimed);
        
        _claimButton.onClick.AddListener(Claim);
    }

    private void Claim()
    {
        _achievementData.Claimed = true;
        _claimButton.interactable = false;
        _claimedImage.gameObject.SetActive(true);
        _audioService.PlaySound(ConstAudio.CoinSound);
        _userInventoryService.AddBalance(_achievementData.Reward);
    }
}
