using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public event Action<Vector3> PointCollected;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            rb2D.simulated = false;
            Vector3 positionToSpawn = this.transform.position;
            PointCollected?.Invoke(positionToSpawn);

            Destroy(gameObject);
        }
    }
}
