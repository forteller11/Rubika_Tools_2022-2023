using System;
using UnityEngine;

public class Class07Exercise03C : MonoBehaviour
{
    public Vector3 Axis = new Vector3(1, 1, 1);
    public Vector3 Point = new Vector3(1, 0, 0);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(Vector3.zero, Axis.normalized);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(Point, 0.02f);
        Gizmos.DrawRay(Vector3.zero, Point);
        
        Vector3 lastPoint = GetPointAtAngle(0);
        for (float i = 5; i < 365; i += 5)
        {
            Color color = Color.HSVToRGB(i / 360, 1f, 1f);

            var currentPoint = GetPointAtAngle(i);
            Gizmos.color = color;
            Gizmos.DrawLine(lastPoint, currentPoint);
            lastPoint = currentPoint;
        }
    }

    private Vector3 GetPointAtAngle(float angle)
    {
        var rotation = Quaternion.AngleAxis(angle, Axis);
        var transformedPoint = rotation * Point;
        return transformedPoint;
    }
}