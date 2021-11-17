using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridgeController : MonoBehaviour
{
    [Header("Ant Propertys")]
    [SerializeField] private GameObject _antCollection;
    [SerializeField] private GameObject _antPF;
    [SerializeField] private int _totalAntCount;
    private int _activeAntCount;
    private float _antWidth;

    [Header("Bridge Live Time")]
    [SerializeField] bool _bridgeHasTimeLimit; // If Bool = true the bridge will only last the set lenght of time in Seconds
    [SerializeField] float _bridgeLifeLimit;
    private float bridgeActivatedTime;

    // Bridge Control variables
    private bool _isBridgeActive = false;
    private Vector3 _bridgePointA;
    private Vector3 _bridgeTargetA;
    private Vector3 _bridgePointB;
    private Vector3 _bridgeTargetB;
    [SerializeField] private float _bridgeMaxLenght;
    private Vector3 _bridgeDirection;
    private float bridgeLenght;
    private bool spawnPointSwitch;
    private float spawnDelay;

    [Header("EventsHandlers")]
    [SerializeField] Collectables _collectables;

    [Header("UI Elements")]
    [SerializeField] private Text _antCountText;

    private void Awake() {
        // allow ant size to be changed and adjust the space covered by bridge
        GameObject Tempant = Instantiate(_antPF, Vector3.zero, Quaternion.identity);
        _antWidth = Tempant.transform.localScale.x;
        Destroy(Tempant);
    }

    private void Start()
    {
        _collectables.AddAnt += AddAnts;
    }

    void Update() {
        _antCountText.text = "Ants in your pants: " + _totalAntCount.ToString();
        _bridgeMaxLenght = (_antWidth * _totalAntCount);
        _activeAntCount = _antCollection.transform.childCount;
        if (_isBridgeActive) {
            _bridgeDirection = (_bridgePointB - _bridgePointA).normalized;
            if (_activeAntCount < _totalAntCount) {
                SpawnAnts();
            }
        }
        if (Time.time - bridgeActivatedTime >= _bridgeLifeLimit && _bridgeHasTimeLimit) {
            KillAnts();
        }
    }

    private void SpawnAnts() {
        if (_isBridgeActive) {
            if (spawnDelay <= 0) {
                if (!spawnPointSwitch) {
                    GameObject ant = Instantiate(_antPF, _bridgePointA, Quaternion.identity, _antCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(_bridgePointA, _bridgeTargetA, _bridgeDirection);
                    spawnPointSwitch = true;
                    spawnDelay = (_antWidth * 2);
                }
                else {
                    GameObject ant = Instantiate(_antPF, _bridgePointB, Quaternion.identity, _antCollection.transform);
                    ant.GetComponent<AntController>().SetTargets(_bridgePointB, _bridgeTargetB, _bridgeDirection);
                    spawnPointSwitch = false;
                    spawnDelay = (_antWidth * 2);
                }
            }
            spawnDelay -= Time.deltaTime;
        }
    }

    public void SetBridgePoints(List<GameObject> bridgePoints) {
        if (bridgePoints.Count >= 2) {
            _isBridgeActive = false;
            KillAnts();
            _bridgePointA = bridgePoints[0].transform.position;
            _bridgePointB = bridgePoints[1].transform.position;
            SetTargetPoints();
        }
    }

    private void SetTargetPoints() {
        _bridgeDirection = _bridgePointB - _bridgePointA;
        bridgeLenght = _bridgeDirection.magnitude;
        if (bridgeLenght > _bridgeMaxLenght) {
            _bridgeTargetA = Vector3.MoveTowards(_bridgePointA, _bridgePointB, _bridgeMaxLenght / 2);
            _bridgeTargetB = Vector3.MoveTowards(_bridgePointB, _bridgePointA, _bridgeMaxLenght / 2);
        }
        else {
            _bridgeTargetA = _bridgePointB;
            _bridgeTargetB = _bridgePointA;
        }
        bridgeActivatedTime = Time.time;
        _isBridgeActive = true;
    }

    public void KillAnts() {
        _isBridgeActive = false;
        foreach(Transform Child in _antCollection.transform) {
            Child.GetComponent<AntController>().dieOnNextTarget = true;
        }
    }

    public void AddAnts(int _newAnts)
    {
        _totalAntCount += _newAnts;
    }
}