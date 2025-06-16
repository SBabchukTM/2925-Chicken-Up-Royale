using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Game.Care;
using UnityEngine;
using Zenject;

public class BoostersDisplayManager : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    private BoosterDisplayFactory _factory;
    
    [Inject]
    private void Construct(BoosterDisplayFactory factory)
    {
        _factory = factory;
    }

    private void Awake()
    {
        foreach (var item in _factory.GetBoosterDisplay())
        {
            item.transform.SetParent(_parent, false);
        }
    }
}
