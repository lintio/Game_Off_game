using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : MonoBehaviour
{
    [SerializeField] private GameObject pfVile;
    [SerializeField] private GameObject pfLinePoint;
    [SerializeField] private LineController lineController;

    Camera mainCam;

    Vector3 handPos;
    private float throwForce;

    public List<GameObject> blobs = new List<GameObject>();

    private void Start()
    {
        mainCam = Camera.main;

        throwForce = 5f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowVile((mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);
        }
    }

    void ThrowVile(Vector3 throwDir)
    {
        GameObject vile = Instantiate(pfVile, transform.position, Quaternion.identity);
        vile.GetComponent<Rigidbody2D>().AddForce(throwDir * throwForce, ForceMode2D.Impulse);
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
        }
        if (blobs.Count >= 2)
        {
            lineController.SetPoints(blobs);
        }
        Debug.Log(blobs);
    }
}
