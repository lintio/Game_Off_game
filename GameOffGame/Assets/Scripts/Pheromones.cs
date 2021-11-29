using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject pfVile;

    [Header("Links to other scripts")]
    AntSystem redAntSystem;
    [SerializeField] private BridgeController bridgeController;
    [SerializeField] private Trajectory_Line trajectoryLine;

    [Header("Throw Stats")]
    [SerializeField] private bool _useThrowForceRamp;
    [SerializeField] private float _MaxThrowForce;
    [SerializeField] private float _MinThrowForce;
    [SerializeField] private float _ThrowForceGenerateSpeed;
    private float throwForce;

    [Header("EventsHandlers")]
    [SerializeField] Collectables _collectables;

    private void Awake()
    {
        redAntSystem = new AntSystem(0);
        _collectables.AddRedAnt += AddAnts;
    }

    private void Update()
    {
        if (_useThrowForceRamp)
        {
            if (Input.GetMouseButton(1))
            {
                throwForce += Time.deltaTime * _ThrowForceGenerateSpeed;
                Mathf.Clamp(throwForce, _MinThrowForce, _MaxThrowForce);
                trajectoryLine.GetThrowForce(throwForce);
            }
            if (Input.GetMouseButtonUp(1))
            {
                throwForce = _MinThrowForce;
            }
        }
        else
        {
            throwForce = _MaxThrowForce;
            trajectoryLine.GetThrowForce(throwForce);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            ThrowVile();
            throwForce = _MinThrowForce;
        }
    }

    void ThrowVile()
    {
        GameObject vile = Instantiate(pfVile, transform.position, transform.rotation);
        vile.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
        Bullet bullet = vile.GetComponent<Bullet>();
        bullet.SetDamageAmount(redAntSystem.GetAntCount());
        bullet.PointCollected += bridgeController.AddBlobToList;
    }

    public void AddAnts(int _newAnts)
    {
        redAntSystem.IncreaseAntCountMax(_newAnts);
    }
}
