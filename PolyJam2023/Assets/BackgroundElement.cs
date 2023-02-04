using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    private GameplayManager _gameplayManager;
    [SerializeField]
    private float _speedMultiplier = 1f;

    public float PlayerPreviousXCoord = -Mathf.Infinity;

    public float Width { get { return _boxCollider2D.size.x; } }

    public BackgroundElement Prefab;
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _gameplayManager = FindObjectOfType<GameplayManager>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (PlayerPreviousXCoord != -Mathf.Infinity)
        {
            float playerXShift = _gameplayManager.CurrentPlayer.transform.position.x - PlayerPreviousXCoord;
            Vector2 translationVector = new Vector2(playerXShift * _speedMultiplier, 0f);
            transform.Translate(translationVector);
        }

        PlayerPreviousXCoord = _gameplayManager.CurrentPlayer.transform.position.x;
    }

}
