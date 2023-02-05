using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerAgingData _playerAgingData;
    private AgeData _currentAgeData;
    public AgeData CurrentAgeData => _currentAgeData;
    [SerializeField]
    private float _agingSpeed;
    private float _age;

    private SpriteRenderer _spriteRenderer;

    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            _animator.SetBool("IsRunning", value);
        }
    }

    public bool IsAlive = true;

    private MyInputAsset _playerInput;

    private Rigidbody2D _rigidbody2D;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;

    private List<GroundTile> _groundTilesInTouch = new List<GroundTile>();
    private bool _jumpButtonDown = false;

    private int _jumpCount = 0;

    private Collider2D _collider;

    private Animator _animator;

    private float _referenceSpeed;

    private Sprite _newSprite;



    // Start is called before the first frame update
    public void Init()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
    }

    public void StartRunning()
    {
        _playerInput.PlayerControls.Enable();
        HandleAge();
        ChangeSprite();
        _referenceSpeed = _currentAgeData.RunningSpeed;
        IsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            Vector3 shift = Vector3.right * _currentAgeData.RunningSpeed * Time.deltaTime;
            transform.Translate(shift);
            HandleJumpGravity();
        }

        if (IsAlive)
        {
            _age += _agingSpeed * Time.deltaTime;
            HandleAge();

            _animator.SetBool("InAir", !CheckIfOnGround());

            if (CheckIfOnGround())
            {
                _jumpCount = 0;
            }

        }
    }

    private void HandleAge()
    {
        for (int i = _playerAgingData.AgeDataList.Count - 1; i >= 0; i--)
        {
            var ageData = _playerAgingData.AgeDataList[i];

            if (ageData.AgeThreshold <= _age)
            {
                if (_currentAgeData != ageData)
                {
                    _currentAgeData = ageData;
                    _animator.SetTrigger("AgeUp");
                    _newSprite = _currentAgeData.AvatarSprite;
                    Debug.Log("Changing age");
                }
                break;
            }
        }
    }

    private void Jump()
    {
        bool canJump = CheckIfOnGround() || _jumpCount < _currentAgeData.JumpComboLimit;
        canJump &= IsAlive && IsRunning;

        if (canJump)
        {
            Debug.Log("JumpCount: " + _jumpCount);
            IsRunning = false;
            _jumpCount++;
            _animator.SetTrigger("Jump");
        }
    }

    public void TakeOff()
    {
        IsRunning = true;
        Vector2 jumpVector = new Vector2(0f, _currentAgeData.JumpForce);
        _rigidbody2D.velocity = jumpVector;
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
        return _rigidbody2D.velocity.y == 0 && _groundTilesInTouch.Count > 0;
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

    public void Die()
    {
        IsRunning = false;
        IsAlive = false;
        _playerInput.Disable();
        _animator.SetTrigger("Die");
        _collider.enabled = false;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.gravityScale = 0.5f;
    }

    public void ChangeSprite()
    {
        _spriteRenderer.sprite = _newSprite;
    }

    private void OnDestroy()
    {
        _playerInput.Dispose();
    }

    public void Struggle()
    {
        _animator.SetTrigger("Struggle");
    }

}
