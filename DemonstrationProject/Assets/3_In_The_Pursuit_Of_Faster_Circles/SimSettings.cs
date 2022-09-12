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
        public float2 BoundsMin;
        public float2 BoundsMax;
        public float2 InitRadiusRange;
        public float MinRadiusUntilDestruction;
        public int2 DebrisOnDestruction;
        public float2 PercentageRadiusOfDebrisRange;
    }
}