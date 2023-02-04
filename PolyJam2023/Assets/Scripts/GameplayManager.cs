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

    [SerializeField]
    private Player _playerPrefab;
    [SerializeField]
    private Transform _initialSpawnPoint;

    private Player _currentPlayer;

    private MyInputAsset _inputAsset;

    [SerializeField]
    private TransformTracker _cameraTransformTracker;

    private void Start()
    {
        _inputAsset = new MyInputAsset();

        _inputAsset.GameControls.Enable();

        _inputAsset.GameControls.Die.performed += ctx =>
        {
            DieAndRespawn();
        };

        _gameOverLabel.gameObject.SetActive(false);
        _tileSpawner.SpawnInitialTiles();
        DieAndRespawn();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _gameOverLabel.gameObject.SetActive(true);
    }

    private void DieAndRespawn()
    {
        Player newPlayer = Instantiate(_playerPrefab, transform);

        if (_currentPlayer != null)
        {
            _currentPlayer.Die();
            newPlayer.transform.position = _currentPlayer.transform.position;
        }
        else
        {
            newPlayer.transform.position = _initialSpawnPoint.transform.position;
        }

        _currentPlayer = newPlayer;
        _cameraTransformTracker.AssignTransform(_currentPlayer.transform);
        newPlayer.Init();
        newPlayer.StartRunning();
    }
}
