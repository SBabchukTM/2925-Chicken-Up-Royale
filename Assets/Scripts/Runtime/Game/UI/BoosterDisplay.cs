using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    public void Initialize(Sprite image)
    {
        _image.sprite = image;
    }
}
