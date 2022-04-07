using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core
{
    public class XpFly : MonoBehaviour
    {
        public Transform From;
        public Transform To;
        
        public Transform Child;
        public float ChildOffset;
        public float Duration;
        public float Scale = 1f;
        public Ease MoveEase = Ease.Linear;
        public Ease OffsetEase = Ease.OutFlash;
        public float RotateAngle = 360f;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        [Button]
        public async Task Test_Fly()
        {
            await Fly(From, To);
        }
        
        public async Task Fly(Transform from, Transform to)
        {
            gameObject.SetActive(true);
            var tr = transform;

            Vector3 startPos = from.position;
            Vector3 endPos = to.position;
            var cross = (Vector3.Cross((startPos + endPos) * 0.5f, Vector3.forward).normalized);
            Child.rotation = Quaternion.identity;
            Child.localPosition = Vector3.zero;
            tr.position = startPos;
            tr.localScale = Vector3.one * Scale;
            tr.DOMove(endPos, Duration).SetEase(MoveEase);
            Child.DOLocalMove(cross * ChildOffset, Duration / 2f).SetEase(OffsetEase).SetLoops(2, LoopType.Yoyo);
            Child.DOLocalRotate(new Vector3(0,0,RotateAngle), Duration , RotateMode.FastBeyond360).SetEase(Ease.Linear);
            tr.DOScale(1/Scale, Duration).SetEase(Ease.Linear);

            await Task.Delay((int)(Duration * 1000f));
            
            gameObject.SetActive(false);
        }
    }
}