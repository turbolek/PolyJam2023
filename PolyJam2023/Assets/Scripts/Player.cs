using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _agingSpeed;
    [SerializeField]
    private float _age;

    [SerializeField]
    private TransformTracker _tracker;

    private bool _isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartRunning();
    }

    public void StartRunning()
    {
        _tracker.AssignTransform(transform);
        _isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            Vector3 shift = Vector3.right * _speed * Time.deltaTime;
            transform.Translate(shift);
        }
    }
}
