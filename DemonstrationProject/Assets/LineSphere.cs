using System;
using Unity.Mathematics;
using UnityEngine;

namespace Exercises._08
{
    public class LineSphere : MonoBehaviour
    {
        public float3 dir;
        public float3 sphereCenter;
        public float sphereRadius;
        public UnityEngine.Object reference;
        public MonoBehaviour reference2;

        public float val;

        private void OnDrawGizmos()
        {
            var rayDir = math.normalizesafe(dir);
            var closestPoint = math.project(sphereCenter, rayDir);
            float closestDistFromSphere = math.distance(closestPoint, sphereCenter);
            val = closestDistFromSphere;
            bool isHit = (closestDistFromSphere <= sphereRadius);
            var color = isHit ? Color.green : Color.red;

            Gizmos.color = color;
            Gizmos.DrawLine(Vector3.zero, closestPoint);
            
            Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
        }
    }
}