using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Componants")]
    private Animator _animator;
    Camera _mainCam;
    [SerializeField] private MovementController movementController;

    [Header("Anitation Veriables")]
    private Vector3 baseLocalScale;
    private bool _flipPlayer;
    private bool _trackMouse;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _mainCam = Camera.main;
        baseLocalScale = transform.localScale;
    }

    private void Update()
    {
        GetMouesDirection();
        // Play walk/run animation
        bool isWakling = movementController._horizontalDirection != 0 ? true : false;
        _animator.SetBool("IsWalking", isWakling);

        FlipPlayer();

        if (Input.GetMouseButton(1))
        {
            _trackMouse = true;
        }
        else
        {
            _trackMouse = false;
        }
    }

    private float GetMouesDirection()
    {
        Vector2 _direction;
        Vector2 MousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        _direction = (MousePos - playerPos).normalized;
        return _direction.x;
    }

    private void FlipPlayer()
    {
        if (_trackMouse)
        {
            if (GetMouesDirection() < -0.001f)
                _flipPlayer = true;
            else if (GetMouesDirection() > 0.001f)
                _flipPlayer = false;
        }
        else if (movementController._horizontalDirection < -0.001f)
            _flipPlayer = true;
        else if (movementController._horizontalDirection > 0.001f)
            _flipPlayer = false;
        if (_flipPlayer)
        {
            transform.localScale = new Vector3(-baseLocalScale.x, baseLocalScale.y, baseLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(baseLocalScale.x, baseLocalScale.y, baseLocalScale.z);
        }
    }
}
