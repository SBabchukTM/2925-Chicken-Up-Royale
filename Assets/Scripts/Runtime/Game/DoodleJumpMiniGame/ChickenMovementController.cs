using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using UnityEngine;
using Zenject;

public class ChickenMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveForce;

    [Inject]
    private IAudioService _audioService;
    
    public void Move(float moveDir)
    {
        if(moveDir == 0)
            return;
        
        _rigidbody2D.AddForce(Vector2.right * (moveDir * _moveForce));
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        foreach (ContactPoint2D contact in other.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                _audioService.PlaySound(ConstAudio.JumpSound);
                break;
            }
        }
    }
}
