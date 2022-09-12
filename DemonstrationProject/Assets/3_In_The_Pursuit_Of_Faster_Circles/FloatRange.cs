using System;
using Unity.Mathematics;

namespace Charly.PursuitFasterCircles.Utils
{
    [Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Unlerp(float t)
        {
            return math.unlerp(Min, Max, t);
        }
        
        public float UnlerpClamped(float t)
        {
            return math.clamp(Unlerp(t), 0, 1);
        }

        public float Lerp(float t)
        {
            return math.lerp(Min, Max, t);
        }
        
        public float LerpClamped(float t)
        {
            return math.clamp(Lerp(t), Min, Max);
        }
    }
}