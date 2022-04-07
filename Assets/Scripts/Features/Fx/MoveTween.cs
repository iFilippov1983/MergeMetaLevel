using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class MoveTween : MonoBehaviour, ITween, IGetTweens
    {
        public Vector3 FromPos;
        public Vector3 ToPos;
        public Transform From;
        public Transform To;
        public AnimationCurve CurveX;
        public AnimationCurve CurveY;
        [Range(0, 4)]
        public float Duration;
        public float Delay;
        public bool ResetInitial = true;
        private bool _stop;

        private Tweener _tweenX;
        private Tweener _tweenY;

        [Button]
        void Stop() => _stop = true;
        
        [Button]
        public void Fly()
        {
            DoTween(Duration);
        }

        public void DoTween(float duration)
        {
            SetInitial();
            if (CurveX != null)
            {
                if(To != null)
                    DoMoveX(duration);
                else
                    DoLocalMoveX(duration);
            }
            if(CurveY != null)
                if(To != null)
                    DoMoveY(duration);
                else
                    DoLocalMoveY(duration);
        }

        private void DoMoveX(float duration) => _tweenX = transform.DOMoveX(ToPos.x, GetDuration(duration)).SetEase(CurveX).SetDelay(Delay);
        private void DoLocalMoveX(float duration) => _tweenX = transform.DOLocalMoveX(ToPos.x, GetDuration(duration)).SetEase(CurveX).SetDelay(Delay);

        private void DoMoveY(float duration) => _tweenY = transform.DOMoveY(ToPos.y, GetDuration(duration)).SetEase(CurveY).SetDelay(Delay);
        private void DoLocalMoveY(float duration) => _tweenY = transform.DOLocalMoveY(ToPos.y, GetDuration(duration)).SetEase(CurveY).SetDelay(Delay);

        private void SetInitial()
        {
            if (From != null)
                FromPos = From.position;
            if (To != null)
                ToPos = To.position;

            if (ResetInitial)
            {
                if (From != null)
                    transform.position = FromPos;
                else
                    transform.localPosition = FromPos;
            }
        }

        public void GetTweens(List<DG.Tweening.Tween> list)
        {
            if(CurveX != null)
                list.Add(_tweenX);
            if(CurveY != null)
                list.Add(_tweenY);
        }

        private float GetDuration(float duration) => Duration != 0 ? Duration : duration;

        private void OnEnable()
        {
            SetInitial();
        }
    }
}