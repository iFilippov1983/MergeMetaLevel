using System;
using System.Threading.Tasks;
using Components;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class MergePlusOneView : ManagedMonobeh<MergePlusOneView>
    {
        public Transform Child;

        public void SetCtx(Action<MergePlusOneView> dispose) => base.SetCtxBase(dispose);
        
        private async void OnEnable()
        {
            Child.DOLocalMove(Vector2.up, 1);
            await Task.Delay(1000);
            Destroy(gameObject);
        }
    }
}