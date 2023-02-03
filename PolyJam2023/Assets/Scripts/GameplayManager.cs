using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private GroundTileSpawner _tileSpawner;

    private void Start()
    {
        _tileSpawner.SpawnInitialTiles();
    }
}
