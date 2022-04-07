using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Core
{
    public class FromToTween : MonoBehaviour, ITween, IGetTweens
    {
        public Transform From;
        public Transform To;
        public AnimationCurve Curve;
        
        [Range(0, 4)]
        public float Duration;

        private Tweener _tweenRotate;
        private Tweener _tweenScale;
        private Tweener _tweenMove;

        public void DoTween(float duration)
        {
            SetInitial();
            var tr = transform;
            _tweenRotate = tr.DORotateQuaternion(To.rotation, GetDuration(duration));
            _tweenScale = tr.DOScale(To.localScale, GetDuration(duration));
            _tweenMove = tr.DOMove(To.position, GetDuration(duration));
        }

        public void GetTweens(List<DG.Tweening.Tween> list)
        {
            list.Add(_tweenRotate);
            list.Add(_tweenScale);
            list.Add(_tweenRotate);
        }

        private void SetInitial()
        {
            var tr = transform;
            tr.rotation = From.rotation;
            tr.localScale = From.localScale;
            tr.position = From.position;
        }

        private float GetDuration(float duration) => Duration != 0 ? Duration : duration;

        private void OnEnable()
        {
            SetInitial();
        }

    }
}