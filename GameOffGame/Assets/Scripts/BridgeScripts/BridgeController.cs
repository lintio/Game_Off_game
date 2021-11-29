using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class BridgeController : MonoBehaviour
{
    private AntSystem antSystem;

    [Header("Ant Properties")]
    [SerializeField] private GameObject _antCollection;
    [SerializeField] private GameObject _antPF;
    private float _antWidth;

    [Header("Bridge Properties")]
    [SerializeField] private GameObject pfLinePoint;
    private List<GameObject> blobs = new List<GameObject>();
    private bool _isBridgeActive = false;

    private Vector3 _bridgePointA;
    private Vector3 _bridgeTargetA;
    private Vector3 _bridgePointB;
    private Vector3 _bridgeTargetB;

    private float _bridgeMaxLenght;
    private Vector3 _bridgeDirection;
    private float bridgeLenght;

    [SerializeField] private float spawnDelay;
    private bool spawnPointSwitch;

    [Header("Bridge Modifiers")]
    [SerializeField] private bool _persistentBridge;
    [SerializeField] bool _bridgeHasTimeLimit;
    [SerializeField] float _bridgeLifeLimit;
    private float bridgeActivatedTime;

    [Header("EventsHandlers")]
    [SerializeField] Collectables _collectables;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _antCountText;

    private void Awake() {
        antSystem = new AntSystem(0);
        // allow ant size to be changed and adjust the space covered by bridge
        GameObject TempAnt = Instantiate(_antPF, Vector3.zero, Quaternion.identity);
        _antWidth = TempAnt.transform.localScale.x;
        Destroy(TempAnt);
    }

    private void Start()
    {
        _collectables.AddBlackAnt += AddAnts;
        antSystem.OnAntCountChanged += UpdateUI;
    }

    public float GetAntCount()
    {
        return antSystem.GetAntCount();
    }

    void UpdateUI(object sender, System.EventArgs e)
    {
        _antCountText.text = "you have " + GetAntCount() + " Black Ants in your pants!";
    }

    void Update() {
        _bridgeMaxLenght = _antWidth * antSystem.GetAntCount();
        if (_isBridgeActive) {
            
            _bridgeDirection = (_bridgePointB - _bridgePointA).normalized;
            if (GetAntCount() != 0) {
                SpawnAnts();
            }
        }
        if (Time.time - bridgeActivatedTime >= _bridgeLifeLimit && _bridgeHasTimeLimit) {
            KillAnts();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            for (int i = 0; i < blobs.Count; i++)
            {
                Destroy(blobs[i]);
            }
            KillAnts();
            blobs.Clear();
        }
    }

    private void SpawnAnts() {
            if (spawnDelay <= 0) {
                antSystem.SetAntsInUse(1);
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

    public void AddBlobToList(Vector3 positionToSpawn)
    {
        GameObject blob = (GameObject)Instantiate(pfLinePoint, positionToSpawn + new Vector3(0, 0, 1), Quaternion.identity);
        blobs.Add(blob);
        if (_persistentBridge)
        {
            if (blobs.Count >= 4)
            {
                Destroy(blobs[1]);
                blobs.RemoveAt(1);
                Destroy(blobs[0]);
                blobs.RemoveAt(0);
                SetBridgePoints(blobs);
            }
        }
        else
        {
            if (blobs.Count >= 3)
            {
                Destroy(blobs[0]);
                blobs.RemoveAt(0);
                SetBridgePoints(blobs);
            }
        }

        if (blobs.Count >= 2)
        {
            SetBridgePoints(blobs);
        }
    }

    public void KillAnts() {
        _isBridgeActive = false;
        foreach(Transform Child in _antCollection.transform) {

            AntController antController = Child.GetComponent<AntController>();
            antController.onAntDie += ReleaseBridgeAnts;
            antController.dieOnNextTarget = true;
        }
    }

    public void ReleaseBridgeAnts(object sender, System.EventArgs e)
    {
        antSystem.ReleaseAntsFromUse(1);
    }

    public void AddAnts(int _newAnts)
    {
        antSystem.IncreaseAntCountMax(_newAnts);
    }
}