using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using UnityEngine;
using Zenject;

public class CoinItem : MonoBehaviour
{
    private DoodleGameData _data;
    private IAudioService _audioService;

    [Inject]
    private void Construct(DoodleGameData data, IAudioService audioService)
    {
        _data = data;
        _audioService = audioService;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out ChickenController chickenController))
        {
            _data.Coins++;
            _audioService.PlaySound(ConstAudio.CoinSound);
            Destroy(gameObject);
        }
    }
}
