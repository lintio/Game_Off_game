using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : MonoBehaviour
{
    [SerializeField] private GameObject pfVile;
    [SerializeField] private GameObject pfLinePoint;
    //[SerializeField] private LineController lineController;
    [SerializeField] private BridgeController bridgeController;

    Camera mainCam;

    public float throwForce;

    public float aimPointTimeOffset;

    public int aimPathPointCount;
    Vector2 direction;
    public GameObject pfAimPathPoint;
    GameObject[] aimPathPoints;

    public List<GameObject> blobs = new List<GameObject>();
    List<Vector3> BlobPos = new List<Vector3>();



    private void Start()
    {
        aimPathPoints = new GameObject[aimPathPointCount];

        for (int i = 0; i < aimPathPointCount; i++)
        {
            aimPathPoints[i] = Instantiate(pfAimPathPoint, transform.position, Quaternion.identity);
        }

        mainCam = Camera.main;
    }

    private void Update()
    {
        Vector2 MousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 firePoint = transform.position;

        direction = (MousePos - firePoint).normalized;

        faceMouse();

        //direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        if (Input.GetMouseButtonDown(0))
        {
            ThrowVile();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            for (int i = 0; i < blobs.Count; i++)
            {
                Destroy(blobs[i]);
                bridgeController.KillAnts();
            }
            blobs.Clear();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            for (int i = 0; i < aimPathPoints.Length; i++)
            {
                aimPathPoints[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < aimPathPoints.Length; i++)
            {
                aimPathPoints[i].SetActive(false);
            }
        }
        for (int i = 0; i < aimPathPoints.Length; i++)
        {
            aimPathPoints[i].transform.position = PointPosition(i * aimPointTimeOffset);
        }

    }

    private void faceMouse()
    {
        transform.right = direction;
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
        //BlobPos.Add(blob.transform.position);
        if (blobs.Count >= 3)
        {
            Destroy(blobs[0]);
            blobs.RemoveAt(0);
            //BlobPos.RemoveAt(0);
            //bridgeController.SetBridgePoints(BlobPos);
            bridgeController.SetBridgePoints(blobs);
        }
        if (blobs.Count >= 2)
        {
            //bridgeController.SetBridgePoints(BlobPos);
            bridgeController.SetBridgePoints(blobs);
        }
    }

    Vector2 PointPosition(float t)
    {
        
        Vector2 currentPointPos = (Vector2)transform.position + (direction.normalized * throwForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPointPos;
    }
}
