using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // New Movement 3rd attempt
    [Header("Componants")]
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement Variables")]
    [SerializeField] private bool useForce;
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _groundLinearDrag;
    public float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [Range(0, 20)]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLinearDrag = 2.5f;
    [Range(0, 5)]
    [SerializeField] private int _extraJumps = 1;
    private int _extraJumpsValue;

    [Header("Better Jump Stats")]
    [Range(0, 10)]
    [SerializeField] private float _fallMultiplier = 2.5f;
    [Range(0, 10)]
    [SerializeField] private float _lowJumpFallMultiplier = 2f;

    [Header("Ground Collision Varialbles")]
    [SerializeField] private float _groundRaycastLenght = 0.2f;
    private bool _onGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;

        if (Input.GetButtonDown("Jump") && (CheckCollisions() || _extraJumpsValue > 0))
        {
            Jump();
        }       

        BetterJumps();

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

    private void BetterJumps()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpFallMultiplier - 1) * Time.deltaTime;
        }
    }

    private bool CheckCollisions()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size, 0f, Vector2.down, _groundRaycastLenght, _groundLayer);

        return raycastHit.collider != null;
    }
}
