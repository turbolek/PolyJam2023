using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GroundTile> _groundTilePrefabs = new List<GroundTile>();
    [SerializeField]
    private int _tileCountLimit = 10;
    [SerializeField]
    private float _tileWidth = 1f;

    private List<GroundTile> _groundTiles = new List<GroundTile>();
    private GroundTile _currenTile;

    public void SpawnInitialTiles()
    {
        for (int i = 0; i < _tileCountLimit; i++)
        {
            var randomPrefab = GetRandomTilePrefab();
            SpawnTile(randomPrefab);
        }

        _currenTile = _groundTiles[0];
    }

    private void SpawnTile(GroundTile _tilePrefab)
    {
        GroundTile lastTile = null;

        if (_groundTiles.Count > 0)
        {
            lastTile = _groundTiles[_groundTiles.Count - 1];
        }

        GroundTile newTile = Instantiate(_tilePrefab, transform);
        _groundTiles.Add(newTile);

        if (lastTile != null)
        {
            newTile.transform.position = lastTile.transform.position + new Vector3(_tileWidth, 0, 0);
        }

        newTile.Init(OnGroundTileEntered);
    }

    public void SpawnNewTile()
    {
        if (_groundTiles.Count > 0)
        {
            var firstTile = _groundTiles[0];
            _groundTiles.RemoveAt(0);
            Destroy(firstTile.gameObject);
        }

        var randomPrefab = GetRandomTilePrefab();
        SpawnTile(randomPrefab);
    }

    public GroundTile GetRandomTilePrefab()
    {
        GroundTile _randomPrefab = _groundTilePrefabs[Random.Range(0, _groundTilePrefabs.Count)];
        return _randomPrefab;

    }

    private void OnGroundTileEntered(GroundTile tile)
    {
        if (_currenTile != null)
        {
            float currentTileX = _currenTile.transform.position.x;
            float newTileX = tile.transform.position.x;

            if (newTileX > currentTileX)
            {
                _currenTile = tile;
                SpawnNewTile();
            }
        }
    }
}
