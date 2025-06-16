using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseItemDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _amountText;

    private int _amount;
    public int Amount => _amount;
    
    public void Initialize(Sprite sprite, int amount)
    {
        _image.sprite = sprite;
        UpdateAmount(amount);
    }

    public void UpdateAmount(int amount)
    {
        _amount = amount;
        _amountText.text = amount.ToString();
    }
}
