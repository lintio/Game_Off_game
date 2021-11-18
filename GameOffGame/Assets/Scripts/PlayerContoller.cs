using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerContoller : MonoBehaviour
{
    // New Movement 3rd attempt
    [Header("Componants")]
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    [Header("Anitation Veriables")]
    private Vector3 baseLocalScale;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private bool useForce;
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _groundLinearDrag;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private int _extraJumps = 1;
    private int _extraJumpsValue;

    [Header("Ground Collision Varialbles")]
    [SerializeField] private float _groundRaycastLenght;
    private bool _onGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponentInChildren<Animator>();
        baseLocalScale = transform.localScale;
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;

        // Player Jump
        if (Input.GetButtonDown("Jump") && (CheckCollisions() || _extraJumpsValue > 0))
        {
            Jump();
        }

        // Play walk/run animation
        bool isWakling = _horizontalDirection != 0 ? true : false;
        _animator.SetBool("IsWalking", isWakling);

        // Flip the player
        if (_horizontalDirection < -0.001f)
            transform.localScale = new Vector3(-baseLocalScale.x, baseLocalScale.y, baseLocalScale.z);
        else if (_horizontalDirection > 0.001f)
            transform.localScale = new Vector3(baseLocalScale.x, baseLocalScale.y, baseLocalScale.z);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        
        if (CheckCollisions())
        {
            _extraJumpsValue = _extraJumps;
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultipler();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        if (useForce)
        {
            _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

            if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
                _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
        else
        {
            transform.position += new Vector3(GetInput().x, 0, 0) * Time.deltaTime * _maxMoveSpeed;
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void Jump()
    {
        if (!_onGround)
            _extraJumpsValue--;

        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void FallMultipler()
    {
        if (_rb.velocity.y > 0)
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }

    private bool CheckCollisions()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f,  Vector2.down, _groundRaycastLenght, _groundLayer);
        
        Color rayColor = raycastHit.collider != null ? rayColor = Color.green : rayColor = Color.red;
        Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundRaycastLenght), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, 0), Vector2.down * (_boxCollider.bounds.extents.y + _groundRaycastLenght), rayColor);
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, _boxCollider.bounds.extents.y + _groundRaycastLenght), Vector2.right * (_boxCollider.bounds.extents.x *2), rayColor);

        return raycastHit.collider != null;
    }
}
