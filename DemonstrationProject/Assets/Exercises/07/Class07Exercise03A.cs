using System;
using UnityEngine;

public class Class07Exercise03A : MonoBehaviour
{
    public Vector3 Pivot;
    public Vector3 Axis = new Vector3(1, 1, 1);
    public float AngleIncrement;

    private void Update()
    {
        transform.RotateAround(Pivot, Axis, AngleIncrement * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(Pivot, 0.2f);
        Gizmos.DrawRay(Pivot, Axis.normalized);
    }
}
