using System;
using Runtime.Game.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketBox : MonoBehaviour
{
    [SerializeField] private Image _itemForSell;
    [SerializeField] private Button _addItemButton;
    [SerializeField] private Button _helpSlotButton;
    [SerializeField] private Button _buySlotButton;
    [SerializeField] private TextMeshProUGUI _buyPriceText;
    [SerializeField] private GameObject _lockGO;
    [SerializeField] private GameObject _helpBG;
    [SerializeField] private GameObject _priceStand;
    [SerializeField] private TextMeshProUGUI _sellPriceText;
    [SerializeField] private Button _collectButton;
    [SerializeField] private TextMeshProUGUI _timeLeftText;

    private int _slotPrice;
    public int SlotPrice => _slotPrice;

    private int _slotID;
    public int SlotID => _slotID;
    
    public event Action<MarketBox> OnPurchasePressed;
    public event Action<MarketBox> OnPlaceItemPressed;
    public event Action<MarketBox> OnCollectPressed;
    
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
        
        _addItemButton.onClick.AddListener(() =>
        {
            OnPlaceItemPressed?.Invoke(this);
        });
        
        _collectButton.onClick.AddListener(() =>
        {
            OnCollectPressed?.Invoke(this);
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
                _lockGO.SetActive(true);
                break;
            case ItemHolderState.Purchased:
                _timeLeftText.transform.parent.gameObject.SetActive(false);
                _lockGO.SetActive(false);
                _priceStand.SetActive(false);
                _addItemButton.gameObject.SetActive(true);
                _collectButton.gameObject.SetActive(false);
                break;
            case ItemHolderState.Occupied:
                _timeLeftText.transform.parent.gameObject.SetActive(true);
                _addItemButton.gameObject.SetActive(false);
                _priceStand.SetActive(true);
                _collectButton.gameObject.SetActive(false);
                break;
            case ItemHolderState.CollectReady:
                _timeLeftText.transform.parent.gameObject.SetActive(false);
                _itemForSell.gameObject.SetActive(false);
                _priceStand.SetActive(true);
                _collectButton.gameObject.SetActive(true);
                break;
        }
    }

    public void SetTimeLeft(double time)
    {
        _timeLeftText.text = Helper.FormatTimeWithHours((float)time);
    }

    public void SetSellData(Sprite sprite, int price)
    {
        _itemForSell.gameObject.SetActive(true);
        _itemForSell.sprite = sprite;
        _sellPriceText.text = price.ToString();
    }
}

public enum ItemHolderState
{
    Purchased,
    NotPurchased,
    Occupied,
    CollectReady
}
