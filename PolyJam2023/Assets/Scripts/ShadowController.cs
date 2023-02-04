using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private GameplayManager _gameplayManager;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _maxPlayerDistance = 20;

    private Vector3 _initialPosition;

    public void Init()
    {
        _initialPosition = transform.position;

    }

    private void Update()
    {
        Vector3 shift = Vector3.right * _speed * Time.deltaTime;
        transform.Translate(shift);

        float distanceToPlayer = Mathf.Abs(_gameplayManager.CurrentPlayer.transform.position.x - transform.position.x);

        if (distanceToPlayer > _maxPlayerDistance)
        {
            transform.position = new Vector3(_gameplayManager.CurrentPlayer.transform.position.x - _maxPlayerDistance, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player hitPlayer = collision.gameObject.GetComponent<Player>();

        if (hitPlayer != null && hitPlayer.IsAlive)
        {
            _gameplayManager.GameOver();
        }
    }

    public void ResetPosition()
    {
        transform.position = _initialPosition;
    }
}
