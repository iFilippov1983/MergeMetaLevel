using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class ExecuteTween : SerializedMonoBehaviour, ITween, IGetTweens
    {
        public List<ITween> Tweens;
        
        [Range(0, 4)]
        public float Duration = 1.88f;

        public bool RunOnEnable = false;

        private void OnEnable()
        {
            if(RunOnEnable)
                DoTween(Duration);
        }

        public void GetTweens(List<DG.Tweening.Tween> list)
        {
            foreach (var tween in Tweens)
            {
                if(tween is IGetTweens)
                    (tween as IGetTweens).GetTweens(list);
            }
        }

        [Button]
        public void DoTween(float duration = 0 )
        {
            var _duration = duration > 0 ? duration : Duration;
            Tweens.ForEach(t => t.DoTween(_duration));
        }

        public async Task AwaitTween()
        {
            gameObject.SetActive(true);
            Tweens.ForEach(t => t.DoTween(Duration));
            await Task.Delay((int)(Duration * 1000));
        }

        public void SetMoveTarget(Transform tr)
        {
            var move = Tweens.Find(t => t is MoveTween) as MoveTween;
            if(move == null)
                return;
            move.To = tr;
        }
    }
}