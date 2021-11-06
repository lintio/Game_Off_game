using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    LineRenderer lr;

    public Transform pointA;
    public Transform pointB;

    Vector3 LinePointA;
    Vector3 LinePointB;
    Vector3 LinePointTarget = new Vector3(0,0,0);

    float speed = 10f;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (LinePointB != LinePointTarget)
        {
            LinePointB = Vector3.MoveTowards(LinePointB, LinePointTarget, speed * Time.deltaTime);
        }
        pointA.position = LinePointA;
        pointB.position = LinePointB;
        lr.SetPosition(0, LinePointA);
        lr.SetPosition(1, LinePointB);
    }

    public void GetNewPoints(List<Vector3> points)
    {
        if (points.Count >= 2)
        {
            LinePointA = points[0];
            LinePointB = LinePointA;
            LinePointTarget = AdjustTarget(points[1]);
        }
        
    }

    private Vector3 AdjustTarget(Vector3 targetPoint)
    {
        Vector2 targetV2 = new Vector2(targetPoint.x, targetPoint.y);
        Vector2 linePointAV2 = new Vector2(LinePointA.x, LinePointA.y);
        RaycastHit2D checkTarget;
        checkTarget = Physics2D.Raycast(LinePointA, -(linePointAV2 - targetV2).normalized, 2.5f);
        if (checkTarget.point != targetV2)
        {
            Debug.Log("Hit " + checkTarget.collider.name + " at " + checkTarget.point);
            return new Vector3(checkTarget.point.x, checkTarget.point.y, 0);
        }
        else
        {
            Debug.Log("No Hit");
            return targetPoint;
        }
    }
}
