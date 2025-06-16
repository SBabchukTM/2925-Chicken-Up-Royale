using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleGameData
{
    private float _time;
    private int _coins;

    public event Action<float> OnTimeChanged;
    public event Action<int> OnCoinsChanged;
    
    public float Time
    {
        get => _time;
        set
        {
            _time = value;
            OnTimeChanged?.Invoke(_time);
        }
    }

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            OnCoinsChanged?.Invoke(_coins);
        }
    }
}
