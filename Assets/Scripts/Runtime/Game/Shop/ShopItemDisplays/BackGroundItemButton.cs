using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundItemButton : ShopItemDisplay
{
    [SerializeField] private Image _checkImage;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private GameObject _coinGO;
    
    private int _id;
    private bool _purchased;
    public int Id => _id;
    
    public void Initialize(int id, bool purchased)
    {
        _id = id;
        _purchased = purchased;

        _coinGO.SetActive(!purchased);
        _priceText.gameObject.SetActive(!purchased);
    }

    public void SetSelected(bool active)
    {
        if(active)
            _statusText.text = "Selected";
        else
        {
            if(_purchased)
                _statusText.text = "Purchased";
        }
    }
}
