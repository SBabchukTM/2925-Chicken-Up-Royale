using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterItemButton : ShopItemDisplay
{
    private int _id;
    
    public int Id => _id;
    
    public void Initialize(int boostedTypeId)
    {
        _id = boostedTypeId;
    }
}
