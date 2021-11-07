using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] GameObject antPF;
    [SerializeField] GameObject AntCollection;
    bool bridgeActive = false;
    Vector3 bridgePointA;
    Vector3 bridgeTargetA;
    Vector3 bridgePointB;
    Vector3 bridgeTargetB;
    [SerializeField] float antWidth; //Need a way to calculate this at Start so if I change the ant size this updates
    float bridgeMaxLenght;
    Vector3 bridgeDirection;
    float bridgeLenght;
    int spawnPointSwitch = 0;
    public float spwanDelayTime;
    float spawnDelay;
    int antCount;
    public int totalAntCount;
    [SerializeField] bool bridgeHasTimeLimit; // If Bool = true the bridge will only last the set lenght of time in Seconds
    [SerializeField] float bridgeLifeTime;
    float bridgeActivatedTime;
    
    private void Start() {
        // ***********************************************************************************
        //  Calculate Ant Width here maybe spawn one then destroy it to get the width from it
        // ***********************************************************************************
    }

    void Update() {
        bridgeMaxLenght = (antWidth * totalAntCount);
        antCount = AntCollection.transform.childCount;
        if (bridgeActive) {
            bridgeDirection = (bridgePointB - bridgePointA).normalized;
            if (antCount < totalAntCount) {
                SpawnAnts();
            }
        }
        if (Time.time - bridgeActivatedTime >= bridgeLifeTime && bridgeHasTimeLimit) {
            KillAnts();
        }
    }

    private void SpawnAnts() {
        if (bridgeActive) {
            if (spawnDelay <= 0) {
                if (spawnPointSwitch == 0) {
                    GameObject ant = Instantiate(antPF, bridgePointA, Quaternion.identity, AntCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(bridgePointA, bridgeTargetA, bridgeDirection);
                    spawnPointSwitch = 1;
                    spawnDelay = (antWidth * 2);
                }
                else {
                    GameObject ant = Instantiate(antPF, bridgePointB, Quaternion.identity, AntCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(bridgePointB, bridgeTargetB, bridgeDirection);
                    spawnPointSwitch = 0;
                    spawnDelay = (antWidth * 2);
                }
            }
            spawnDelay -= Time.deltaTime;
        }
    }

    public void SetBridgePoints(List<GameObject> bridgePoints) {
        if (bridgePoints.Count >= 2) {
            bridgeActive = false;
            KillAnts();
            bridgePointA = bridgePoints[0].transform.position;
            bridgePointB = bridgePoints[1].transform.position;
            SetTargetPoints();
        }
    }

    private void SetTargetPoints() {
        bridgeDirection = bridgePointB - bridgePointA;
        bridgeLenght = bridgeDirection.magnitude;
        if (bridgeLenght > bridgeMaxLenght) {
            bridgeTargetA = Vector3.MoveTowards(bridgePointA, bridgePointB, bridgeMaxLenght / 2);
            bridgeTargetB = Vector3.MoveTowards(bridgePointB, bridgePointA, bridgeMaxLenght / 2);
        }
        else {
            bridgeTargetA = bridgePointB;
            bridgeTargetB = bridgePointA;
        }
        bridgeActivatedTime = Time.time;
        bridgeActive = true;
    }

    public void KillAnts() {
        bridgeActive = false;
        foreach(Transform Child in AntCollection.transform) {
            Child.GetComponent<AntController>().dieOnNextTarget = true;
        }
    }
}