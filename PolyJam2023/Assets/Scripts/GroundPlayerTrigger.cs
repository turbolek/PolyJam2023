using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlayerTrigger : MonoBehaviour
{
    private Action _onEnteredCallback;

    private bool _initialized = false;

    public void Init(Action onEnteredCallback)
    {
        _onEnteredCallback = onEnteredCallback;
        _initialized = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_initialized)
        {
            _onEnteredCallback.Invoke();
        }
    }
}
