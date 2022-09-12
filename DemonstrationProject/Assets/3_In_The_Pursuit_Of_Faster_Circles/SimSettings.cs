using System;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.PursuitFasterCircles
{
    [Serializable]
    public struct SimSettings
    {
        public int SpawnNumber;
        public float2 InitVelocityRange;
        public float2 BoundsX;
        public float2 BoundsY;
        public float2 InitRadiusRange;
        public float MinRadiusUntilDestruction;
    }
}