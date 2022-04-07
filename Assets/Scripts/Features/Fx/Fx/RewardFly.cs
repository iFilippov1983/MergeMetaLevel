using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Features.Fx
{
    public class RewardFly : MonoBehaviour
    {
        public Transform Target;
        public Transform Child;
        
        public RewardFlyConfig Config;

        [Button]
        public void FlyTo(Vector3? TargetPos)
        {
            var target = TargetPos.HasValue ? TargetPos.Value : Target.position;
            
            transform.DOKill();
            
            var appearPos = new Vector3(Rnd(Config.Radus), Rnd(Config.Radus));
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            
            var duration = Config.AppearTime;
            transform.DOScale(1, duration).SetEase(Config.Scale);
            transform.DOMove(transform.position + appearPos, duration).SetEase(Config.Move);
            transform.DOMoveX(target.x, Config.FlyTime).SetEase(Config.X).SetDelay(duration  + Config.FlyTimeOffset);
            transform.DOMoveY(target.y, Config.FlyTime).SetEase(Config.Y).SetDelay(duration + Config.FlyTimeOffset)
                .OnComplete(() => gameObject.SetActive(false));
        }

        // private float Rnd(float value) => Random.Range(-value, value);
        private float Rnd(float value) => Random.Range(value*0.6f, value) * (Random.value > 0.5f ? -1 : 1);
    }
}