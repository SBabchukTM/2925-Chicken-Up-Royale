using System.Collections.Generic;
using System.Linq;
using Runtime.Game.UI.Screen;
using UnityEngine;
using Random = UnityEngine.Random;

public class WormsSpawner : MonoBehaviour
{
    [SerializeField] private int _targetProgress;
    
    [SerializeField] private float _minSpawnDelay;
    [SerializeField] private float _maxSpawnDelay;
    
    [SerializeField] private List<WormHole> _wormHoles;

    [SerializeField] private FoodGameScreen _foodGameScreen;
    
    private float _nextSpawnTime;

    private void Awake()
    {
        UpdateSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            UpdateSpawnTime();
            SpawnWorm();
        }
    }

    private void UpdateSpawnTime()
    {
        _nextSpawnTime = Time.time + Random.Range(_minSpawnDelay, _maxSpawnDelay);
    }

    private void SpawnWorm()
    {
        var holes = _wormHoles.Where(x => !x.InAnim).ToList();

        var hole = holes[Random.Range(0, holes.Count)];
        
        if(hole)
            hole.PlaySpawnAnim();
    }
}
