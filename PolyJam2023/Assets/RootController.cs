using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    [SerializeField]
    private float _holdTime;

    private bool _usedUp = false;

    private Player _heldPlayer;

    private float _holdStartTime;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_usedUp)
        {
            return;
        }

        Player hitPlayer = collision.gameObject.GetComponent<Player>();

        if (hitPlayer != null && hitPlayer.IsAlive && hitPlayer.IsRunning)
        {
            _heldPlayer = hitPlayer;
            StartHolding();
        }
    }

    private void StartHolding()
    {
        _audioSource.Play();
        _heldPlayer.IsRunning = false;
        _usedUp = true;
        _holdStartTime = Time.time;
        _heldPlayer.Rigidbody2D.velocity = Vector2.zero;
        _heldPlayer.Rigidbody2D.gravityScale = 0f;
        _heldPlayer.Struggle();
    }

    private void ReleasePlayer()
    {
        _audioSource.Stop();
        _heldPlayer.IsRunning = _heldPlayer.IsAlive;
        _heldPlayer = null;
    }

    private void Update()
    {
        if (_heldPlayer != null && Time.time >= _holdStartTime + _holdTime)
        {
            ReleasePlayer();
        }
    }
}
