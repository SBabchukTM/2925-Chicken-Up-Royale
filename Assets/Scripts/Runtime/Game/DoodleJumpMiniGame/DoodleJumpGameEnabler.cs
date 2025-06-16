using Runtime.Core.Factory;
using UnityEngine;
using Zenject;

public class DoodleJumpGameEnabler : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private GameObjectFactory _factory;
    private CameraFollow _cameraFollow;
    
    private GameObject _spawnedLevel;

    [Inject]
    private void Construct(GameObjectFactory factory)
    {
        _factory = factory;
        _cameraFollow = FindObjectOfType<CameraFollow>();
    }
    
    public void SpawnLevel()
    {
        _spawnedLevel = _factory.Create(_prefab);
        _spawnedLevel.gameObject.SetActive(true);
        
        _cameraFollow.ResetPosition();
        _cameraFollow.Enable(true);
    }

    public void DestroyLevel()
    {
        _cameraFollow.Enable(false);
        Destroy(_spawnedLevel);
    }
}
