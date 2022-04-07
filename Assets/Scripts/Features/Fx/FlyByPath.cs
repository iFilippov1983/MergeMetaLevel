using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class FlyByPath : MonoBehaviour, ITween
    {
        public Transform From;
        public Transform To;
        public AnimationCurve CurveX;
        public AnimationCurve CurveY;
        [Range(0, 4)]
        public float Duration;

        private bool _stop;


        [Button]
        void Stop() => _stop = true;

        [Button]
        public async void RepeatFly()
        {
            _stop = false;
            while (!_stop)
            {
                DoTween(Duration);
                await Task.Delay((int)(Duration * 1000));
            }
        }
        
        public void DoTween(float duration)
        {
            transform.position = From.position;
            transform.DOKill();
            transform.DOMoveX(To.position.x, GetDuration(duration)).SetEase(CurveX);
            transform.DOMoveY(To.position.y, GetDuration(duration)).SetEase(CurveY);
        }

        private float GetDuration(float duration) => Duration != 0 ? Duration : duration;
    }
}