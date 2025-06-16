using System;
using Runtime.Game.Tools;
using UnityEngine;
using Zenject;

public class WashingInputProvider : ITickable
{
    public event Action OnInput;

    private Vector3 _prevTouchPosition;

    private bool _enabled;

    public void SetEnabled(bool enabled) => _enabled = enabled;
    
    public void Tick()
    {
        if(!_enabled)
            return;
        
        if (!AnyInput())
            return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Moved)
            OnInput?.Invoke();
    }

    private bool AnyInput() => Input.touchCount > 0 && Helper.IsPointerOverChickenBathing();
}
