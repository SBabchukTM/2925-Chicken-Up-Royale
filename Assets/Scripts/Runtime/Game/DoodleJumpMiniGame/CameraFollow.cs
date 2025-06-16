using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private const float ZPos = -10;
    
    [SerializeField] private float _moveTime = 0.1f;

    private Transform _target;
    private bool _enable = false;

    private void Update()
    {
        if (!_enable)
            return;
        
        Vector3 targetPos = _target.position;
        targetPos.z = 0;
        targetPos.x = 0;

        transform.DOMoveY(targetPos.y, _moveTime).SetLink(gameObject);
    }

    public void ResetPosition()
    {
        transform.position = Vector3.forward * ZPos; 
    }

    public void Enable(bool enable)
    {
        _target = FindObjectOfType<ChickenController>().transform;
        _enable = enable;
    }
}
