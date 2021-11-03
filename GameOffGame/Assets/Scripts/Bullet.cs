using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2D;
    public GameObject pfBlob;

    public Pheromones pheromones;


    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb2D.simulated = false;
            Vector3 positionToSpawn = this.transform.position;
            pheromones.AddBlobToList(positionToSpawn);
            Destroy(gameObject);
        }
    }
}
