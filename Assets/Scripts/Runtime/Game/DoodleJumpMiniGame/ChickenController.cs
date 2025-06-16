using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChickenController : MonoBehaviour
{
    [SerializeField] private ChickenMovementController _chickenMovementController;
    [SerializeField] private ChickenVisuals _chickenVisuals;
    
    private ChickenInputProvider _chickenInputProvider;

    private float _lastMoveDir;
    
    [Inject]
    private void Construct(ChickenInputProvider inputProvider)
    {
        _chickenInputProvider = inputProvider;
    }

    private void Update()
    {
        _lastMoveDir = _chickenInputProvider.GetInput();
        _chickenVisuals.UpdateVisuals(_lastMoveDir);
    }

    private void FixedUpdate()
    {
        _chickenMovementController.Move(_lastMoveDir);    
    }
}
