using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Components
{
    public class MergeItemLifeView : ManagedMonobeh<MergeItemLifeView>
    {
        public Transform Child;

        public void SetCtx(Action<MergeItemLifeView> dispose) => base.SetCtxBase(dispose);
        
        public async Task FlyTo( int posX, int posY, MergeVisualConfig visualConfig, int index, int rand)
        {
            var tr = transform;
            var startPos = tr.position;
            var endPos = new Vector3(posX, posY, 0);
            var duration = FlyDuration(startPos, endPos);

            //Child.localScale = Vector3.zero;
            //// await Task.Delay((int)( index*0.3f*1000));
            //Child.DOScale(1f, 0.2f).SetEase(Ease.OutFlash);
            var renders = Child.GetComponentsInChildren<Renderer>();
            foreach (var render in renders)
                render.sortingLayerName = "MergeFx";
            
            AnimateFly();
            
            await Task.Delay(duration.ToMs());

            foreach (var render in renders)
                render.sortingLayerName = "MergeFx";
            
            void AnimateFly()
            {
                var offsetEase = Ease.OutFlash;
                var moveEase = Ease.Linear;
            
                var offset = MinusOneOrOne(rand);
                var cross = (Vector3.Cross((startPos + endPos) * 0.5f, Vector3.forward).normalized);
                tr.position = startPos;
                tr.DOMove(endPos, duration).SetEase(moveEase);
                
                Child.DOLocalMove(cross * offset, duration / 2f).SetEase(offsetEase).SetLoops(2, LoopType.Yoyo);
            }
        }

        private static float FlyDuration(Vector3 startPos, Vector3 endPos)
        {
            var dist = Vector3.Distance(startPos, endPos);
            var duration = dist switch
            {
                _ when dist < 3 => 0.8f,
                _ when dist < 5 => 1.2f,
                _ when dist < 7 => 1.4f,
                _ => 1.6f,
            };
            // duration = 1f;
            return duration * Random.Range(0.8f, 1.1f);
        }

        // private static int MinusOneOrOne(int index) => (index % 2) * 2 - 1;
        private static int MinusOneOrOne(int index) => index == 0 ? -1 : 1;
    }
}