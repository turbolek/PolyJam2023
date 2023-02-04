using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileSpawner : SerializedMonoBehaviour
{
    [SerializeField]
    private List<GroundTile> _initialTileSetup = new List<GroundTile>();
    [SerializeField]
    private List<TileSpawnData> _groundTilePrefabs = new List<TileSpawnData>();

    public class TileSpawnData
    {
        public GroundTile TilePrefab;
        public float SpawnChance;
    }

    [SerializeField]
    private int _tileCountLimit = 10;
    [SerializeField]
    private float _tileWidth = 1f;

    [SerializeField]
    private float _tileDestroyDistance = 5f;

    private List<GroundTile> _groundTiles = new List<GroundTile>();
    private GroundTile _currenTile;

    [SerializeField]
    private Transform _tileParent;

    public void SpawnInitialTiles()
    {
        _groundTiles.Clear();

        int tilesToSpawn = _tileCountLimit;
        for (int i = 0; i < _initialTileSetup.Count; i++)
        {
            SpawnTile(_initialTileSetup[i], _tileParent);
            tilesToSpawn--;
        }

        while (tilesToSpawn > 0)
        {
            SpawnTile(GetRandomTilePrefab(), _tileParent);
            tilesToSpawn--;
        }

        _currenTile = _groundTiles[0];
    }

    private void SpawnTile(GroundTile _tilePrefab, Transform tileParent)
    {
        GroundTile lastTile = null;

        if (_groundTiles.Count > 0)
        {
            lastTile = _groundTiles[_groundTiles.Count - 1];
        }

        GroundTile newTile = Instantiate(_tilePrefab, tileParent);
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
        SpawnTile(randomPrefab, _tileParent);

        ClearOldTiles();
    }

    public GroundTile GetRandomTilePrefab()
    {
        float chanceSum = 0f;

        foreach (TileSpawnData spawnData in _groundTilePrefabs)
        {
            chanceSum += spawnData.SpawnChance;
        }

        float chanceRoll = Random.Range(0f, chanceSum);
        float coveredChance = 0f;

        foreach (TileSpawnData spawnData in _groundTilePrefabs)
        {
            coveredChance += spawnData.SpawnChance;
            if (coveredChance >= chanceRoll)
            {
                return spawnData.TilePrefab;
            }
        }

        return null;
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
