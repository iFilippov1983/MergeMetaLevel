using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Tween
{
    public class TweenTest : MonoBehaviour
    {
        public DG.Tweening.Tween _tween;
        public Sequence _sequence;

        public MoveTween MoveTween;
        public ScaleTween MoveTween2;
        // public ExecuteTween Tweens;
        public float Duration = 2;

        [Range(0,2), OnValueChanged("SetTime")]
        public float Time;

        private List<DG.Tweening.Tween> _tweens;


        [Button]
        private void DOTween()
        {
            _tweens = new List<DG.Tweening.Tween>();
            MoveTween.DoTween(Duration);
            MoveTween2.DoTween(Duration);
            MoveTween.GetTweens(_tweens);
            MoveTween2.GetTweens(_tweens);
            GoToTime(0);
        }

        void SetTime()
        {
            Debug.Log(">>>");
            GoToTime(Time);
        }

        private void GoToTime(float time)
        {
            _tweens.ForEach(t => t.Goto(time));
        }
    }
}