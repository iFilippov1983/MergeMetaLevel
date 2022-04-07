using UnityEngine;

namespace Features.Fx
{
    public class RewardFlyConfig : ScriptableObject
    {
        public float Radus = 1;
        public AnimationCurve Move;
        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Scale;
        public float AppearTime = 2f;
        public float FlyTime = 2f;
        public float FlyTimeOffset = -0.3f;
        public float DelayPerFx = 0.1f;
    }
}