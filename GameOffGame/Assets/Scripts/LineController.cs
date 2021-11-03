using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private List<Vector3> points = new List<Vector3>();

    [SerializeField] Transform pointATransform;
    [SerializeField] Transform pointBTransform;

    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 pointT;

    public bool lineActive;

    float speed = 10f;

    public bool doExtend;
    public bool doRetract;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        pointA = new Vector3(0, 0, 0);
        pointB = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        pointATransform.position = pointA;
        pointBTransform.position = pointB;
        if (doExtend)
        {
            if (!doRetract)
            {
                Debug.Log("Extending");
                pointB = Vector3.MoveTowards(pointB, pointT, speed * Time.deltaTime);
                if (pointB == pointT)
                {
                    Debug.Log("Done Extending");
                    doExtend = false;
                }
            }
        }
        if (doRetract)
        {

                Debug.Log("Retracting");
                pointA = Vector3.MoveTowards(pointA, pointB, speed * Time.deltaTime);
                if (pointA == pointB)
                {
                    Debug.Log("Done Retracting");
                    doRetract = false;
                }
        }
            lr.SetPosition(0, pointA);
            lr.SetPosition(1, pointB);
    }

    public void RemoveLine()
    {
        doRetract = true;
        lineActive = false;
        points.Clear();
    }

    public void SetPoints(List<GameObject> blobs)
    {
        if (points.Count >= 2)
            points.Clear();
        for (int i = 0; i < blobs.Count; i++)
        {
            points.Add(blobs[i].transform.position);
        }
        if (!lineActive)
        {
            pointA = points[0];
            pointB = points[0];
            pointT = points[1];
            lineActive = true;
            doExtend = true;
        }
        else
        {
            doRetract = true;
            pointB = points[0];
            pointT = points[1];
            doExtend = true;
        }
    }

    public float GetWidth()
    {
        return lr.startWidth;
    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        return positions;
    }

    void Extend()
    {
        pointBTransform.position = Vector3.MoveTowards(pointA, pointB, speed * Time.deltaTime);
        if (pointBTransform.position == pointB)
            doExtend = false;
    }

    void Retract()
    {
        pointATransform.position = Vector3.MoveTowards(pointA, pointB, speed * Time.deltaTime);
        if (pointATransform.position == pointB)
            doRetract = false;
    }
}
