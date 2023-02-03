using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _groundTilePrefabs = new List<GameObject>();
    [SerializeField]
    private int _tileCountLimit = 10;
    [SerializeField]
    private float _tileWidth = 1f;

    private List<GameObject> _groundTiles = new List<GameObject>();

    public void SpawnInitialTiles()
    {
        for (int i = 0; i < _tileCountLimit; i++)
        {
            var randomPrefab = GetRandomTilePrefab();
            SpawnTile(randomPrefab);
        }
    }

    private void SpawnTile(GameObject _tilePrefab)
    {
        GameObject lastTile = null;

        if (_groundTiles.Count > 0)
        {
            lastTile = _groundTiles[_groundTiles.Count - 1];
        }

        GameObject newTile = Instantiate(_tilePrefab, transform);
        _groundTiles.Add(newTile);

        if (lastTile != null)
        {
            newTile.transform.position = lastTile.transform.position + new Vector3(_tileWidth, 0, 0);
        }
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

    public GameObject GetRandomTilePrefab()
    {
        GameObject _randomPrefab = _groundTilePrefabs[Random.Range(0, _groundTilePrefabs.Count)];
        return _randomPrefab;

    }
}
