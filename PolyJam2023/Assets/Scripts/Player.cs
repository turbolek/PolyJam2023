using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private MyInputAsset _playerInput;

    private Rigidbody2D _rigidbody2D;

    private List<GroundTile> _groundTilesInTouch = new List<GroundTile>();
    private bool _jumpButtonDown = false;

    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _playerInput = new MyInputAsset();

        _playerInput.PlayerControls.Jump.performed += ctx =>
        {
            _jumpButtonDown = true;
            Jump();
        };

        _playerInput.PlayerControls.Jump.canceled += ctx =>
        {
            _jumpButtonDown = false;
        };

        StartRunning();
    }

    public void StartRunning()
    {
        _playerInput.PlayerControls.Enable();
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
            HandleJumpGravity();
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

    private void Jump()
    {
        if (CheckIfOnGround())
        {
            Vector2 jumpVector = new Vector2(0f, _currentAgeData.JumpForce);
            _rigidbody2D.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GroundTile groundTile = collision.collider.GetComponent<GroundTile>();

        if (groundTile != null && !_groundTilesInTouch.Contains(groundTile))
        {
            _groundTilesInTouch.Add(groundTile);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GroundTile groundTile = collision.collider.GetComponent<GroundTile>();

        if (groundTile != null && _groundTilesInTouch.Contains(groundTile))
        {
            _groundTilesInTouch.Remove(groundTile);
        }
    }

    private bool CheckIfOnGround()
    {
        return _groundTilesInTouch.Count > 0;
    }

    private void HandleJumpGravity()
    {
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = _currentAgeData.FallGravity;
        }
        else if (_rigidbody2D.velocity.y > 0 && !_jumpButtonDown)
        {
            _rigidbody2D.gravityScale = _currentAgeData.LowJumpGravity;
        }
        else
        {
            _rigidbody2D.gravityScale = 1f;
        }
    }
}
