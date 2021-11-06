using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] GameObject antPF;
    [SerializeField] GameObject AntCollection;

    [SerializeField] Vector3 bridgePointA;
    [SerializeField] Vector3 bridgePointB;

    int point = 0;

    Vector3 direction; // the direction between bridgePoints A and B

    bool bridgeActive = false; // if the bridge has 2 points

    public float spwanDelayTime;
    float spawnDelay;

    [SerializeField] int antCount; // amount of ants spawned
    public int totalAntCount; // amount of ants allowed to spawn
    [SerializeField] List<GameObject> ants = new List<GameObject>();

    float birdgeActiveTime;
    float bridgeActivationTime;

    // Update is called once per frame
    void Update()
    {
        antCount = AntCollection.transform.childCount;
        if (bridgeActive)
        {
            direction = (bridgePointB - bridgePointA).normalized;
            if (antCount < totalAntCount)
            {
                SpawnAnts();
            }
        }
    }

    private void SpawnAnts()
    {
        if (bridgeActive)
        {
            if (spawnDelay <= 0)
            {
                if (point == 0)
                {
                    GameObject ant = Instantiate(antPF, bridgePointA, Quaternion.identity, AntCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(bridgePointB, bridgePointA, direction);
                    point = 1;
                    //ants.Add(ant);
                    spawnDelay = spwanDelayTime;
                }
                else
                {
                    GameObject ant = Instantiate(antPF, bridgePointB, Quaternion.identity, AntCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(bridgePointA, bridgePointB, direction);
                    point = 0;
                    //ants.Add(ant);
                    spawnDelay = spwanDelayTime;
                }
            }
            spawnDelay -= Time.deltaTime;
        }
    }

    public void SetBridgePoints(List<GameObject> bridgePoints)
    {
        if (bridgePoints.Count >= 2)
        {
            bridgeActive = false;
            KillAnts();
            bridgePointA = bridgePoints[0].transform.position;
            bridgePointB = bridgePoints[1].transform.position;
            bridgeActive = true;
        }
    }

    public void KillAnts()
    {
        bridgeActive = false;
        foreach(Transform Child in AntCollection.transform)
        {
            Child.GetComponent<AntController>().dieOnNextTarget = true;
        }
    }
}
