using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardDisplay : MonoBehaviour
{
    [SerializeField] private Image _collectStatusImage;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private Image _bigRewardImage;
    
    [SerializeField] private Button _collectButton;
    [SerializeField] private GameObject _lockGO;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private TextMeshProUGUI _collectStatusText;
    [SerializeField] private TextMeshProUGUI _dayText;
    
    public event Action<DailyRewardDisplay, int> OnCollected;

    private Image _targetImage;

    public void SetVisuals(Sprite bgSprite, Sprite claimButtonSprite, Sprite rewardSprite)
    {
        _collectStatusImage.sprite = bgSprite;
        _collectButton.image.sprite = claimButtonSprite;
        _targetImage.sprite = rewardSprite;
    }
    
    public void Initialize(bool collected, bool canCollect, int reward, int day)
    {
        _collectButton.interactable = !collected && canCollect;
        _dayText.text = $"Day {day}";
        
        _lockGO.SetActive(!canCollect && !collected);
        
        _rewardText.gameObject.SetActive(reward > 0);
        _rewardText.text = reward.ToString();
        
        _targetImage = reward == 0 ? _bigRewardImage : _rewardImage;
        _bigRewardImage.gameObject.SetActive(reward == 0);
        _rewardImage.gameObject.SetActive(reward > 0);
        
        _collectStatusText.text = collected ? "Claimed" : "Claim";
        _collectButton.onClick.AddListener(() => OnCollected?.Invoke(this, reward));
    }

    public void ClaimReward()
    {
        _collectStatusText.text = "Claimed";
        _collectButton.interactable = false;
    }
}
