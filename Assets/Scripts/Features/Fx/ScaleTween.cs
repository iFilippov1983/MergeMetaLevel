using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class ScaleTween : MonoBehaviour, ITween, IGetTweens
    {
        public float From;
        public float To;
        public Transform FromTransform;
        public Transform ToTransform;
        
        public AnimationCurve Curve;
        [Range(0, 4)]
        public float Duration;
        public float Delay;
        public bool ResetInitial = true;

        private Tweener _tween;

        [Button]
        public void DoTween(float duration)
        {
            SetInitial();
            _tween = transform.DOScale(To, GetDuration(duration)).SetDelay(Delay).SetEase(Curve);
        }

        private void SetInitial()
        {
            if (FromTransform != null)
                From = FromTransform.localScale.x;
            if (ToTransform != null)
                To = ToTransform.localScale.x;
            
            if(ResetInitial)
                transform.localScale = Vector3.one * From;
        }

        public void GetTweens(List<DG.Tweening.Tween> list)
        {
            list.Add(_tween);
        }

        private float GetDuration(float duration) => Duration != 0 ? Duration : duration;

        private void OnEnable()
        {
            SetInitial();
        }

    }
}