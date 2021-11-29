using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class AntController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Color> antColor = new List<Color>();
    [SerializeField] private float speed = 0.5f;

    public event EventHandler onAntDie;

    private bool isActive;
    private Vector3 bridgePointA;
    private Vector3 bridgePointB;
    private Vector3 targetPoint;
    private Vector3 direction;

    public bool dieOnNextTarget = false;

    [Obsolete]
    void Awake()
    {
        _spriteRenderer.color = antColor[index: Random.RandomRange(0,5)];
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (!dieOnNextTarget) {
                if (transform.position == targetPoint) {
                    swapTargets();
                }
            }
            else {
                if (targetPoint != bridgePointA)
                    targetPoint = bridgePointA;
                else if (transform.position == targetPoint)
                {
                    onAntDie?.Invoke(this, EventArgs.Empty);
                    Destroy(gameObject);
                }
            }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        }
    }

    private void swapTargets()
    {
            if (targetPoint == bridgePointA)
                targetPoint = bridgePointB;
            else
                targetPoint = bridgePointA;
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
