using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class AntController : MonoBehaviour
{
    Vector3 bridgePointA;
    Vector3 bridgePointB;

    public SpriteRenderer sr;

    public List<Color> antColor = new List<Color>();

    Vector3 targetPoint;

    Vector3 direction;

    float speed = 0.5f;

    bool isActive;

    public bool dieOnNextTarget = false;

    // Start is called before the first frame update

    [Obsolete]
    void Start()
    {
        sr.GetComponent<SpriteRenderer>().color = antColor[index: Random.RandomRange(0,5)];
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!dieOnNextTarget)
            {
                if (transform.position == targetPoint)
                {
                    swapTargets();
                }
            }
            else
            {
                if (targetPoint != bridgePointA)
                {
                    targetPoint = bridgePointA;
                }
                else if (transform.position == targetPoint)
                {
                    Destroy(gameObject);
                }
            }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        }
    }

    private void swapTargets()
    {
            if (targetPoint == bridgePointA) {
                targetPoint = bridgePointB;
            }
            else {
                targetPoint = bridgePointA;
            }
    }

    public void SetTargets(Vector3 bridgePointA, Vector3 bridgePointB, Vector3 direction)
    {
        this.bridgePointA = bridgePointA;
        this.bridgePointB = bridgePointB;
        this.targetPoint = bridgePointB;
        this.direction = direction;
        isActive = true;
    }
}
