using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory_Line : MonoBehaviour
{
    [Header("Aim Points Setup")]
    [SerializeField] private float aimPointTimeOffset;
    [SerializeField] private int aimPathPointCount;
    [SerializeField] private GameObject pf_AimPathObject;

    [SerializeField] private Vector2 _direction;
    private GameObject[] _aimPathObjects;
    Camera _mainCam;

    private float _throwForce;

    void Start()
    {
        _mainCam = Camera.main;
        _aimPathObjects = new GameObject[aimPathPointCount];

        for (int i = 0; i < aimPathPointCount; i++)
        {
            _aimPathObjects[i] = Instantiate(pf_AimPathObject, transform.position, Quaternion.identity);
        }

    }

    void Update()
    {
        Vector2 MousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 firePoint = transform.position;

        _direction = (MousePos - firePoint).normalized;

        

        faceMouse();

        if (Input.GetMouseButton(1))
        {
            for (int i = 0; i < _aimPathObjects.Length; i++)
            {
                _aimPathObjects[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < _aimPathObjects.Length; i++)
            {
                _aimPathObjects[i].SetActive(false);
            }
        }
        for (int i = 0; i < _aimPathObjects.Length; i++)
        {
            _aimPathObjects[i].transform.position = PointPosition(i * aimPointTimeOffset);
        }
    }

    private void faceMouse()
    {
        transform.right = _direction;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (_direction.normalized * _throwForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPointPos;
    }

    public void GetThrowForce(float throwForce)
    {
        _throwForce = throwForce;
    }
}
