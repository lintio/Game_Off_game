using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private List<Vector3> points = new List<Vector3>();

    [SerializeField] Transform pointATransform;
    [SerializeField] Transform pointBTransform;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 pointT;

    private bool lineActive;

    float speed = 10f;
    public float lineLife;
    float lineCountdown;

    private bool doExtend;
    private bool doRetract;
    private bool lineExtended;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        pointA = new Vector3(0, 0, 0);
        pointB = new Vector3(0, 0, 0);
        
    }

    private void Update()
    {
        if (lineExtended)
        {
            lineCountdown -= Time.deltaTime;
            if (lineCountdown <= 0)
            {
                RemoveLine();
            }
        }
        pointATransform.position = pointA;
        pointBTransform.position = pointB;
        if (doExtend)
        {
            if (!doRetract)
            {
                pointB = Vector3.MoveTowards(pointB, pointT, speed * Time.deltaTime);
                if (pointB == pointT)
                {
                    doExtend = false;
                    lineExtended = true;
                }
            }
        }
        if (doRetract)
        {
                pointA = Vector3.MoveTowards(pointA, pointB, speed * Time.deltaTime);
                if (pointA == pointB)
                {
                    doRetract = false;
                    lineExtended = false;
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
        lineExtended = false;
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
        lineCountdown = lineLife;
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
}
