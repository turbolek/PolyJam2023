using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GroundTile> _initialTileSetup = new List<GroundTile>();
    [SerializeField]
    private List<GroundTile> _groundTilePrefabs = new List<GroundTile>();
    [SerializeField]
    private int _tileCountLimit = 10;
    [SerializeField]
    private float _tileWidth = 1f;

    [SerializeField]
    private float _tileDestroyDistance = 5f;

    private List<GroundTile> _groundTiles = new List<GroundTile>();
    private GroundTile _currenTile;

    public void SpawnInitialTiles()
    {
        int tilesToSpawn = _tileCountLimit;
        for (int i = 0; i < _initialTileSetup.Count; i++)
        {
            SpawnTile(_initialTileSetup[i]);
            tilesToSpawn--;
        }

        while (tilesToSpawn > 0)
        {
            SpawnTile(GetRandomTilePrefab());
            tilesToSpawn--;
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

        var randomPrefab = GetRandomTilePrefab();
        SpawnTile(randomPrefab);

        ClearOldTiles();
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

    private void ClearOldTiles()
    {
        for (int i = 0; i < _groundTiles.Count; i++)
        {
            var tile = _groundTiles[i];
            if (tile.transform.position.x < _currenTile.transform.position.x && Vector3.Distance(tile.transform.position, _currenTile.transform.position) >= _tileDestroyDistance)
            {
                _groundTiles.RemoveAt(i);
                Destroy(tile.gameObject);
            }
        }
    }
}
