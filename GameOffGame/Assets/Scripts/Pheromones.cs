using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : MonoBehaviour
{
    [SerializeField] private GameObject pfVile;
    [SerializeField] private GameObject pfLinePoint;
    [SerializeField] private LineController lineController;

    Camera mainCam;

    private float throwForce;

    Vector2 direction;
    public GameObject pfAimPathPoint;
    public GameObject[] aimPathPoints;
    public int aimPathPointCount;

    public List<GameObject> blobs = new List<GameObject>();



    private void Start()
    {
        aimPathPoints = new GameObject[aimPathPointCount];

        for (int i = 0; i < aimPathPointCount; i++)
        {
            aimPathPoints[i] = Instantiate(pfAimPathPoint, transform.position, Quaternion.identity);
        }

        mainCam = Camera.main;
        throwForce = 20f;
    }

    private void Update()
    {
        direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            ThrowVile();
        }
        if (Input.GetButtonDown("Jump"))
        {
            lineController.RemoveLine();
            for (int i = 0; i < blobs.Count; i++)
            {
                Destroy(blobs[i]);
            }
            blobs.Clear();
        }

        for (int i = 0; i < aimPathPoints.Length; i++)
        {
            aimPathPoints[i].transform.position = PointPosition(i * 0.1f);
        }
    }

    void ThrowVile()
    {
        GameObject vile = Instantiate(pfVile, transform.position, transform.rotation);
        vile.GetComponent<Rigidbody2D>().velocity = transform.right * throwForce;
        vile.GetComponent<Bullet>().pheromones = this;
    }

    public void AddBlobToList(Vector3 positionToSpawn)
    {
        GameObject blob = (GameObject)Instantiate(pfLinePoint, positionToSpawn + new Vector3(0, 0, 1), Quaternion.identity);
        blobs.Add(blob);
        if (blobs.Count >= 3)
        {
            Destroy(blobs[0]);
            blobs.RemoveAt(0);
            lineController.SetPoints(blobs);
        }
        if (blobs.Count >= 2)
        {
            lineController.SetPoints(blobs);
        }
    }

    Vector2 PointPosition(float t)
    {
        return (Vector2)transform.position + (direction * throwForce * t) + 0.5f * Physics2D.gravity * (t * t);
    }
}
