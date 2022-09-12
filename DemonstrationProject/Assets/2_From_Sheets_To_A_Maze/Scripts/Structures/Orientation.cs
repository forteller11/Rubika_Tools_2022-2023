using System;
using Unity.Mathematics;

namespace Charly.SheetsToMaze
{
    [Serializable]
    public struct Orientation
    {
        public float3 Position;
        public quaternion Rotation;
        public float3 Scale;

        public Orientation(float3 position, quaternion rotation, float3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public float4x4 ToTRS()
        {
            return float4x4.TRS(Position, Rotation, Scale);
        }

    }
}