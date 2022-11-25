using System;
using UnityEngine;

public class Class06Exercise03 : MonoBehaviour
{
    public Transform StartObject;
    public Transform EndObject;
    public Transform ObjectToMove;
    [Range(0,1)] public float PercentageToMove;

    private void OnValidate()
    {
        Vector3 startPosition = StartObject.position;
        Vector3 endPosition = EndObject.position;
        
        Vector3 startToEnd = endPosition - startPosition;
        Vector3 amountToMoveFromStart = startToEnd * PercentageToMove;
        Vector3 finalPosition = startPosition + amountToMoveFromStart;
        
        ObjectToMove.transform.position = finalPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(StartObject.position, EndObject.position);
    }
}
