using System;
using Unity.Mathematics;

namespace Charly.PursuitFasterCircles.Utils
{
    public class MathUtils
    {
        public static void DecomposeTR(in float4x4 matrix, out float3 translation, out quaternion rotation)
        {
            translation = matrix.c3.xyz;
            rotation = new quaternion(matrix);
        }
        
        public static void DecomposeTRFast(in float4x4 matrix, out float3 translation, out float3x3 rotation)
        {
            translation = matrix.c3.xyz;
            rotation = new float3x3(matrix);
        }

        public static float AngleBetween(float3 v1, float3 v2)
        {
            return AngleBetweenFast(math.normalizesafe(v1), math.normalizesafe(v2));
        }
        
        public static float AngleBetweenFast(float3 v1Normalized, float3 v2Normalized)
        {
            float dot = math.dot(v1Normalized, v2Normalized);
            return math.acos(dot);
        }
        
        public static quaternion QuaternionFromToFast(float3 fromNormalized, float3 toNormalized)
        {
            float angleBetween = AngleBetweenFast(fromNormalized, toNormalized);
            float3 axis = math.cross(fromNormalized, toNormalized);
            var fromToRotation = quaternion.AxisAngle(axis, angleBetween);

            return fromToRotation;
        }
        
        public static quaternion QuaternionFromTo(float3 from, float3 to)
        {
            return QuaternionFromToFast(math.normalizesafe(from), math.normalizesafe(to));
        }

        public static float3 ExtractScales(float4x4 m)
        {
            return new float3(
                math.length(m.c0.xyz),
                math.length(m.c1.xyz),
                math.length(m.c2.xyz));
        }
        
        public static float3 ExtractPosition(float4x4 m)
        {
            return m.c3.xyz;
        }
        
        public static void Deconstruct(in float4x4 m, out float3 translate, out quaternion rotation, out float3 scale)
        {
            translate = m.c3.xyz;
            rotation = new quaternion(m);
            scale = ExtractScales(m);
        }
        
        public static Orientation Deconstruct(in float4x4 m)
        {
            Deconstruct(m, out var pos, out var rotation, out var scale);
            return new Orientation(pos, rotation, scale);
        }
        

        public static float4x4 NonGeometricLerp(float4x4 a, float4x4 b, float t)
        {
            return new float4x4(
                math.lerp(a.c0, b.c0, t),
                math.lerp(a.c1, b.c1, t),
                math.lerp(a.c2, b.c2, t),
                math.lerp(a.c3, b.c3, t)
            );
        }
        
        public static float4x4 DeconstructLerpReconstruct(float4x4 a, float4x4 b, float t)
        {
            Deconstruct(a, out var posA, out var rotA, out var scaleA);
            Deconstruct(b, out var posB, out var rotB, out var scaleB);
            
            var pos = math.lerp(posA, posB, t);
            var rot = math.slerp(rotA, rotB, t); //todo [Performance] use nlerp instead?
            var scale = math.lerp(scaleA, scaleB, t);
            
            return float4x4.TRS(pos, rot, scale);
        }
        
        public static Orientation Lerp(Orientation a, Orientation b, float t)
        {
            var pos = math.lerp(a.Position, b.Position, t);
            var rot = math.slerp(a.Rotation, b.Rotation, t); //todo [Performance] use nlerp instead?
            var scale = math.lerp(a.Scale, b.Scale, t);
            
            return new Orientation(pos, rot, scale);
        }
        
    }
}