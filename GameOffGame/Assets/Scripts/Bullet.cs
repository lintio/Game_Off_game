using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2D;
    [SerializeField] private int baseDamageAmount = 50;
    [SerializeField] private int damageAmount;
    [SerializeField] private int duration = 10;

    public event Action<Vector3> PointCollected;
    public event Action<int, int> Combat;


    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void SetDamageAmount(float antCount)
    {
        if (antCount <= baseDamageAmount)
        {
            damageAmount = (int)antCount;
        }
        else
        {
            damageAmount = baseDamageAmount;
        }
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

        if (collision.CompareTag("Enemy"))
        {
            
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.DamageOverTime(damageAmount, duration);
            Combat?.Invoke(damageAmount, duration);
            Destroy(gameObject);
        }
    }
}
