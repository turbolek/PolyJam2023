using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private GroundTileSpawner _tileSpawner;

    [SerializeField]
    private TMP_Text _gameOverLabel;

    private void Start()
    {
        _gameOverLabel.gameObject.SetActive(false);
        _tileSpawner.SpawnInitialTiles();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _gameOverLabel.gameObject.SetActive(true);
    }
}
