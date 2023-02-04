using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private GameplayManager _gameplayManager;

    [SerializeField]
    private float _speed;

    private void Update()
    {
        Vector3 shift = Vector3.right * _speed * Time.deltaTime;
        transform.Translate(shift);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _gameplayManager.GameOver();
    }

}
