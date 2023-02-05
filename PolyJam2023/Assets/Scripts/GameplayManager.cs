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
    public Player CurrentPlayer => _currentPlayer;

    private MyInputAsset _inputAsset;

    [SerializeField]
    private TransformTracker _cameraTransformTracker;

    [SerializeField]
    private Transform _spawnedContentParent;

    [SerializeField]
    private ShadowController _shadowController;

    [SerializeField]
    private BackgroundElement[] _backgroundElementPrefabs;

    [SerializeField]
    private TMP_Text _scoreValue;
    [SerializeField]
    private TMP_Text _scoreLabel;

    private float _score;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _gameOverAudioClip;

    [SerializeField]
    private AudioSource _musicAudioSource;

    [SerializeField]
    private AudioClip _firstMusic;
    [SerializeField]
    private AudioClip _loopMusic;
    [SerializeField]
    private AudioClip _gameoverMusic;

    private bool _gameOver = false;

    private void Start()
    {
        _inputAsset = new MyInputAsset();
        _audioSource = GetComponent<AudioSource>();


        _inputAsset.GameControls.Die.performed += ctx =>
        {
            DieAndRespawn();
        };

        _inputAsset.GameControls.Restart.performed += ctx =>
        {
            ResetGame();
        };

        _shadowController.Init();
        _cameraTransformTracker.Init();

        ResetGame();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _gameOverLabel.gameObject.SetActive(true);
        _inputAsset.GameControls.Die.Disable();
        _inputAsset.PlayerControls.Disable();

        _inputAsset.GameControls.Restart.Enable();
        _audioSource.PlayOneShot(_gameOverAudioClip);

        _musicAudioSource.clip = _gameoverMusic;
        _musicAudioSource.loop = false;
        _musicAudioSource.Play();

        _gameOver = true;
    }

    private void DieAndRespawn()
    {
        Player newPlayer = Instantiate(_playerPrefab, _spawnedContentParent);

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

    private void ResetGame()
    {
        _score = 0f;
        Time.timeScale = 1f;
        _inputAsset.GameControls.Restart.Disable();

        for (int i = _spawnedContentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_spawnedContentParent.GetChild(i).gameObject);
        }

        _cameraTransformTracker.ResetPosition();
        _shadowController.ResetPosition();
        _currentPlayer = null;
        _gameOverLabel.gameObject.SetActive(false);
        _inputAsset.GameControls.Enable();
        _tileSpawner.SpawnInitialTiles();
        SpawnbackgroundElements();
        DieAndRespawn();


        _musicAudioSource.clip = _firstMusic;
        _musicAudioSource.loop = false;
        _musicAudioSource.Play();

        _gameOver = false;
    }

    private void SpawnbackgroundElements()
    {
        foreach (BackgroundElement backgroundprefab in _backgroundElementPrefabs)
        {
            Instantiate(backgroundprefab, _spawnedContentParent);
        }
    }

    private void Update()
    {
        if (_currentPlayer.IsAlive && !_gameOver)
        {
            _score += _currentPlayer.CurrentAgeData.PointsPerSecond * Time.deltaTime;
            _scoreValue.text = _score.ToString("F0");

            if (!_musicAudioSource.isPlaying)
            {
                _musicAudioSource.clip = _loopMusic;
                _musicAudioSource.loop = true;
                _musicAudioSource.Play();
            }
        }
    }
}
