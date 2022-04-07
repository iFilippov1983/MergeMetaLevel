using UnityEngine;

namespace Utils
{
    public static class AnimationCurveExtensions
    {
        public static void Scale(this AnimationCurve curve, float value)
        {
            var keyframes = curve.keys;
            for (int j = 0; j < keyframes.Length; j++)
                keyframes[j].value *= value;  
        }
    }
}