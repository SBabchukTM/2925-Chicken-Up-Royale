using System;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncubatorNest : MonoBehaviour
{
    [SerializeField] private Image _eggImage;
    [SerializeField] private Button _placeEggButton;
    [SerializeField] private Button _helpSlotButton;
    [SerializeField] private Button _buySlotButton;
    [SerializeField] private TextMeshProUGUI _buyPriceText;
    [SerializeField] private GameObject _helpBG;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Button _claimButton;

    private int _slotPrice;
    public int SlotPrice => _slotPrice;

    private int _slotID;
    public int SlotID => _slotID;
    
    public event Action<IncubatorNest> OnPurchasePressed;
    public event Action<IncubatorNest> OnPlaceItemPressed;
    public event Action<IncubatorNest> OnClaimPressed;
    
    private void Awake()
    {
        _helpSlotButton.onClick.AddListener(() =>
        {
            _helpBG.SetActive(!_helpBG.activeSelf);
        });
        
        _buySlotButton.onClick.AddListener(() =>
        {
            OnPurchasePressed?.Invoke(this);
        });
        
        _placeEggButton.onClick.AddListener(() =>
        {
            OnPlaceItemPressed?.Invoke(this);
        });
        
        _claimButton.onClick.AddListener(() =>
        {
            OnClaimPressed?.Invoke(this);
        });
    }

    public void Initialize(int price, int slotID)
    {
        _slotPrice = price;
        _slotID = slotID;
        
        _buyPriceText.text = price.ToString();
    }
    
    public void UpdateState(ItemHolderState state)
    {
        switch (state)
        {
            case ItemHolderState.NotPurchased:
                _helpSlotButton.gameObject.SetActive(true);
                break;
            case ItemHolderState.Purchased:
                _eggImage.gameObject.SetActive(false);
                _helpSlotButton.gameObject.SetActive(false);
                _placeEggButton.gameObject.SetActive(true);
                _claimButton.gameObject.SetActive(false);
                _timeText.transform.parent.gameObject.SetActive(false);
                break;
            case ItemHolderState.Occupied:
                _placeEggButton.gameObject.SetActive(false);
                _claimButton.gameObject.SetActive(false);
                _timeText.transform.parent.gameObject.SetActive(true);
                break;
            case ItemHolderState.CollectReady:
                _timeText.transform.parent.gameObject.SetActive(false);
                _claimButton.gameObject.SetActive(true);
                break;
        }
    }

    public void UpdateTimeLeft(double time)
    {
        _timeText.text = Helper.FormatTimeWithHours((float)time);
    }
    
    public void SetVisuals(Sprite sprite)
    {
        _eggImage.gameObject.SetActive(true);
        _eggImage.sprite = sprite;
    }
}
