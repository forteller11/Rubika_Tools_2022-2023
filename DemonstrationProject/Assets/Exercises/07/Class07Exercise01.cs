using System;
using UnityEngine;

public class Class07Exercise01 : MonoBehaviour
{
    public bool UseEulers;
    [Space]
    public Vector3 Euler;
    [Space]
    public float Angle;
    public Vector3 Axis = Vector3.right;


    private void OnValidate()
    {
        if (UseEulers)
        {
            transform.rotation = Quaternion.Euler(Euler);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(Angle,Axis);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, Axis.normalized);
    }
}
