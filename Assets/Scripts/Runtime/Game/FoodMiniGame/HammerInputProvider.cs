using System;
using Runtime.Game.Tools;
using UnityEngine;
using Zenject;

public class HammerInputProvider : ITickable
{
    private bool _enabled = false;

    public event Action<WormHole> OnWormHoleClicked;
    
    public void Tick()
    {
        if(!_enabled)
            return;
        
        if(!AnyInput())
            return;
        
        WormHole hole = Helper.IsPointerOverWormHole();
        
        if(hole)
            OnWormHoleClicked?.Invoke(hole);
    }
    
    public void Enable(bool enable) => _enabled = enable;
    
    private bool AnyInput() => Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
}
