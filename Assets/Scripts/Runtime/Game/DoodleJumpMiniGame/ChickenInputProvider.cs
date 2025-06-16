using Runtime.Game.Tools;
using UnityEngine;
using Zenject;

public class ChickenInputProvider : IInitializable
{
    private Camera _camera;
    public void Initialize()
    {
        _camera = Camera.main;
    }

    public float GetInput()
    {
        if(!AnyInput())
            return 0;
        
        Touch touch = Input.GetTouch(0);
        Vector3 worldPos = GetTouchWorldPos(touch.position);
        return worldPos.x > 0 ? 1 : -1;
    }

    private bool AnyInput() => Input.touchCount > 0 && !Helper.IsPointerOverUIElement();

    private Vector3 GetTouchWorldPos(Vector3 touchPos) => _camera.ScreenToWorldPoint(touchPos);
}
