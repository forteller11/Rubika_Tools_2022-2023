using System;
using UnityEngine;

public class Class07Exercise03B : MonoBehaviour
{
    public Vector3 Pivot;
    public Vector3 Axis = new Vector3(1, 1, 1);
    public Vector3 Point;
    public float Angle;

    private void OnDrawGizmosSelected()
    {
        //Transformation
        var rotation = Quaternion.AngleAxis(Angle, Axis);
        Vector3 transformedPoint = Point - Pivot; //make point relative to pivot
        transformedPoint = rotation * transformedPoint; //rotate
        transformedPoint += Pivot; //put point back into world space (no longer relative to pivot)
        
        //Visualization
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transformedPoint, 0.1f);
        Gizmos.DrawLine(Pivot, transformedPoint);
        
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(Point, 0.1f);
        Gizmos.DrawLine(Vector3.zero, Point);
        
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(Pivot, 0.05f);
        Gizmos.DrawLine(Pivot-Axis.normalized, Pivot+Axis.normalized);
    }
}