using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private Action<GroundTile> _onEnterCallback;
    [SerializeField]
    private GroundPlayerTrigger _playerTrigger;

    public void Init(Action<GroundTile> onEnterCallback)
    {
        _onEnterCallback = onEnterCallback;
        _playerTrigger.Init(OnTileEntered);
    }

    private void OnTileEntered()
    {
        _onEnterCallback?.Invoke(this);
    }
}
