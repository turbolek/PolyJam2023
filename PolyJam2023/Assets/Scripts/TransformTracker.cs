using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    [SerializeField]
    private Transform _transformToTrack;
    private Vector3 _initaialPosition;

    private float _xOffset;

    private void Start()
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
        transform.position = new Vector3(_transformToTrack.position.x - _xOffset, _initaialPosition.y, _initaialPosition.z);
    }
}
