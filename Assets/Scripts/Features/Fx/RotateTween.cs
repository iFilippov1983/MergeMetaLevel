using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class RotateTween : MonoBehaviour, ITween, IGetTweens
    {
        public Vector3 From;
        public Vector3 To;
        public AnimationCurve Curve;
        [Range(0, 4)]
        public float Duration;

        private Tweener _tween;

        public void DoTween(float duration)
        {
            SetInitial();
            _tween = transform.DOLocalRotate(To, GetDuration(duration));
        }

        private void SetInitial()
        {
            transform.localRotation = Quaternion.Euler(From);
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