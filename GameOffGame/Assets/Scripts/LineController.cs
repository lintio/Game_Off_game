using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private List<Transform> points = new List<Transform>();

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    private void Update()
    {
        if (points.Count >= 2)
        {
            lr.SetPositions(points.ConvertAll(n => n.position - new Vector3(0, 0, 5)).ToArray());
        }
    }

    public void SetPoints(List<GameObject> blobs)
    {
        if (points.Count > 0)
        {
            points.Clear();
        }
        for (int i = 0; i < blobs.Count; i++)
        {
            points.Add(blobs[i].transform);
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
}
