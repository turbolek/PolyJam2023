using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerAgingData _playerAgingData;
    private AgeData _currentAgeData;
    [SerializeField]
    private float _agingSpeed;
    private float _age;

    [SerializeField]
    private TMP_Text _ageLabel;

    [SerializeField]
    private TransformTracker _tracker;

    private SpriteRenderer _spriteRenderer;

    private bool _isRunning = false;
    private

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartRunning();
    }

    public void StartRunning()
    {
        HandleAge();
        _tracker.AssignTransform(transform);
        _isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            Vector3 shift = Vector3.right * _currentAgeData.RunningSpeed * Time.deltaTime;
            transform.Translate(shift);
            _age += _agingSpeed * Time.deltaTime;
            _ageLabel.text = "Age: " + _age.ToString("F0");
            HandleAge();
        }
    }

    private void HandleAge()
    {
        foreach (AgeData ageData in _playerAgingData.AgeDataList)
        {
            if (ageData.AgeThreshold <= _age)
            {
                _currentAgeData = ageData;
            }
            else
            {
                break;
            }
        }

        _spriteRenderer.sprite = _currentAgeData.AvatarSprite;
    }
}
