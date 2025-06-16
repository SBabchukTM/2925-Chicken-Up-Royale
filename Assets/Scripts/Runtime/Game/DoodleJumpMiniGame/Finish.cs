using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public static event Action OnPlayerFinished;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent(out ChickenController chickenController))
            OnPlayerFinished?.Invoke();
    }
}
