using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    private Transform _transformToTrack;
    private Vector3 _initaialPosition;

    private float _xOffset;
    private Vector3 _initialPosition;

    public void Init()
    {
        _initaialPosition = transform.position;
    }

    public void AssignTransform(Transform targetTransform)
    {
        _transformToTrack = targetTransform;
        _xOffset = targetTransform.position.x - transform.position.x;
    }

    private void Update()
    {
        if (_transformToTrack != null)
        {
            transform.position = new Vector3(_transformToTrack.position.x - _xOffset, _initaialPosition.y, _initaialPosition.z);
        }
    }

    public void ResetPosition()
    {
        _transformToTrack = null;
        transform.position = _initialPosition;
    }
}
