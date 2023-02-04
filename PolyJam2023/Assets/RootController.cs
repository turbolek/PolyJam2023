using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    [SerializeField]
    private GameplayManager _gameplayManager;

    private void Start()
    {
        _gameplayManager = FindObjectOfType<GameplayManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player hitPlayer = collision.gameObject.GetComponent<Player>();

        if (hitPlayer != null && hitPlayer.IsRunning)
        {
            _gameplayManager.GameOver();
        }
    }
}
