using System;
using System.Threading.Tasks;
using Components;
using Components.Merge;
using Configs;
using Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Core
{
    public class MergeItemView : ManagedMonobeh<MergeItemView>//, IMergeView
    {
        // DATA
        [InlineEditor]
        public MergeItemConfig Config;
        public MergeItemProfileData Data;
        
        // VIEW
        public SpriteRenderer SpriteRenderer;
        public SpriteRenderer DisabledBg;
        public SpriteRenderer DisabledBg2;
        public GameObject ProgressContainer;
        public GameObject CanMergeFx;
        public Slider RedProgress;
        public Slider Progress;
        public TextMeshProUGUI Text;
        [HideInInspector] public Transform Transform;
        public float GrayScaleRatio = 0.5f;
        private bool _inAnimateProgress;
        private int _animatedProgressCount;
        private bool _collapsing;

        // public async Task Animate(string name)
        // {
        //     
        // }
        //
        // public void StopAnimate(string name)
        // {
        //     
        // }
        //
        // public void SetImage(string name)
        // {
        //     
        // }
        //
        // public Transform ChildByName(string name)
        // {
        //     
        // }


        public void SetCtx(MergeItemProfileData data , Action<MergeItemView> cbDispose)
        {
            base.SetCtxBase(cbDispose);
            
            Data = data;
            Config = data.config;
            HideProgress();
            // Progress.gameObject.SetActive(false);
            Transform = transform;
            
            RedProgress.value = 0;
            Progress.value = 0;
        }
        

        [Button]
        public async Task WaitLifeGeneration()
        {
            ProgressContainer.gameObject.SetActive(true);
            await DoProgressImpl(0, 1, 1);
            ProgressContainer.gameObject.SetActive(false);
        }

        [Button]
        public async Task AnimateCollapseToBubble()
        {
            
        }
        
        public async Task AnimateGeneration()
        {
            SpriteRendererTransform.DOKill();
            SpriteRendererTransform.DOScale(0.8f, 0.2f).SetEase(Ease.InOutFlash).OnComplete(() 
                => SpriteRendererTransform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            
            await Task.Delay(300);
        }

        private Transform SpriteRendererTransform 
            => SpriteRenderer.transform;

        [Button]
        public async Task AnimateDisappear()
        {
            if(_collapsing)
                return;
            
            SpriteRendererTransform.DOKill();
            SpriteRendererTransform.DOScale(0.4f, 0.3f).SetEase(Ease.InOutFlash); 
            await Task.Delay(300);
        }

        
        [Button]
        public async Task DoProgressImpl(float from, float to, float duration)
        {
            Progress.value = from;
            if (duration == 0)
                duration = 0.01f;

            Progress.DOKill();
            await Progress.DOValue(to, duration).AsyncWaitForCompletion();
        }

        public void DisableBg()
        {
            DisabledBg.gameObject.SetActive(false);
            DisabledBg2.gameObject.SetActive(false);
        }
        
        public void SetSortOrder(int order) 
            => SpriteRenderer.sortingOrder = order;

        public void SetOnFxLayer()
        {
            SpriteRenderer.sortingLayerName = Constants.Layers.Fx;
            gameObject.layer = LayerMask.NameToLayer("UI");
        } 
        


        [Button(ButtonSizes.Large)]
        public void ToData()
        {
            var xy = transform.localPosition;
            Data.x = Mathf.RoundToInt(xy.x);
            Data.y = Mathf.RoundToInt(xy.y);
            Data.config = Config;
        }

        public async void AnimateUnlocksEditor()
        {
            ShowProgressImmidiate();
            UpdateProgressDataImmidiate();
            
            UpdateLockedView();
            await Task.Delay(500);
            HideProgress();
        }
        public async Task AnimateUnlocks(Action cbAnimateFx)
        {
            UpdateProgressDataImmidiate();
            
            if(_inAnimateProgress)
                return;
            _inAnimateProgress = true;
            
            ShowProgressImmidiate();

            await AnimateGreenProgressLoop(cbAnimateFx);
            
            HideProgress();
            
            if(!Data.locked)
                await AnimateUnlock(cbAnimateFx);
            
            _inAnimateProgress = false;
        }

        private async Task AnimateGreenProgressLoop(Action cbAnimateFx)
        {
            await Task.Delay(100);

            if (_animatedProgressCount < Data.unlockCount)
            {
                // _animatedProgressCount++;
                _animatedProgressCount = Data.unlockCount;
                await AnimateProgress(_animatedProgressCount);
            }

            if (_animatedProgressCount < Data.unlockCount)
                await AnimateGreenProgressLoop(cbAnimateFx);
        }

        private async Task AnimateUnlock(Action cbAnimateFx)
        {
            UpdateLockedView();
            cbAnimateFx?.Invoke();
            await Task.Delay(300);
        }

        private async Task AnimateProgress(int unlockCount)
        {
            // await DoProgressImpl((Data.unlockCount - 1f) / Data.lockCount, (float) Data.unlockCount / Data.lockCount, 1);
            await DoProgressImpl((unlockCount - 1f) / Data.lockCount, (float) unlockCount / Data.lockCount, 1);
        }

        private void ShowProgressImmidiate()
        {
            ProgressContainer.gameObject.SetActive(true);
            // Progress.gameObject.SetActive(true);
            // Text.gameObject.SetActive(true);
            // RedProgress.gameObject.SetActive(true);
        }

        private void HideProgress()
        {
            // ProgressContainer.DOFade(0, 0.3f);
            ProgressContainer.gameObject.SetActive(false);
            // Text.gameObject.SetActive(false);
            // Progress.gameObject.SetActive(false);
            // RedProgress.gameObject.SetActive(false);
        }
        // private void HideGreenProgress()

        private void UpdateProgressDataImmidiate()
        {
            Text.text = $"{Data.unlockCount}/{Data.lockCount}";
            RedProgress.value = 0;//(Data.unlockCount) / Data.lockCount;
        }

        [Button(ButtonSizes.Large)]
        public void UpdateLockedView()
        {
            SetLocked(Data.locked);
        }
        
        [Button(ButtonSizes.Large)]
        public void SetCanMergeFx(bool active)
        {
            CanMergeFx.SetActive(active);
        }

        private void SetLocked(bool locked)
        {
            SetLocketRenderer(locked);
            SetLockedBg(locked);
        }

        private void SetLockedBg(bool locked)
        {
            DisabledBg.gameObject.SetActive(locked);
            DisabledBg2.gameObject.SetActive(locked);
        }

        private void SetLocketRenderer(bool locked)
        {
            var grayRatio = locked ? GrayScaleRatio : 0;
            SpriteRenderer.material.SetFloat("_EffectAmount", grayRatio);
        }

        [Button(ButtonSizes.Large)]
        public void ApplyFromData()
        {
            transform.localPosition = new Vector3(Data.x, Data.y);
            SpriteRenderer.sprite = Config.Sprite;
            UpdateLockedView();
            HideProgress();
        }

        public void PosToData(int x, int y)
        {
            Data.x = x;
            Data.y = y;
        }


        public async void CollapseTo(Vector3 to, MergeVisualConfig visualConfig, ViewFactoryService viewFactoryService, Action afterFly)
        {
            _collapsing = true;
            
            var t = SpriteRendererTransform;
            t.DOKill();
            
            t
                .DOMove(to,  visualConfig.CollapseFlyDuration/1000f)
                .SetEase(visualConfig.CollapseEasing);
            
            await Task.Delay( visualConfig.CollapseScaleDelay); 
            
            t
                .DOScale(visualConfig.CollapseScaleRatio, visualConfig.CollapseScaleDuration / 1000f)
                .SetEase(visualConfig.CollapseScaleEasing);
                // .OnComplete(() => t.gameObject.SetActive(false));
        }

        public async void AnimateClear(int delay, Action afterFly)
        {
            SetLocketRenderer(false);
            await Task.Delay(delay + Random.Range(0, 100));
            SetLockedBg(false);
            afterFly?.Invoke();
        }
        public void StopHighlight(/*bool immidiate*/)
        {
            if(isDestroyed || _collapsing)
                return;
            
            var t = SpriteRendererTransform;
            t.DOKill(true);
            // if (immidiate)
            // t.localPosition = Vector3.zero;
            // else
            t.DOLocalMove(Vector3.zero, 0.2f);

            SetCanMergeFx(false);
        }
        
        public void StartHighlight(Vector3 targetPos)
        {
            if(isDestroyed || _collapsing)
                return;

            var t = SpriteRendererTransform;
            var direction = targetPos - t.position;
            direction.Normalize();
            t.DOLocalMove(t.localPosition + direction * 0.3f, 0.4f).SetLoops(-1, LoopType.Yoyo);

            SetCanMergeFx(true);
        }
        
        public async void AnimateGeneratedDrop(MergeVisualConfig visualConfig, MergeItemView view, int posX, int posY, Vector2 targetPos, int sortOrder = 0)
        {
            var tr = view.transform;
            var spriteRenderer = view.SpriteRenderer;
         
            var pos =  new Vector3(posX, posY);
            var dist = Vector2.Distance(targetPos, pos);
            int flyTime = (int) (Mathf.Ceil(dist) * visualConfig.GeneratedFlySpeed);
            flyTime = Mathf.Max(flyTime, (int) (visualConfig.GeneratedFlySpeed * 2.5f) );
            
            tr.localScale = Vector3.zero;
            tr.position = pos;
            
            tr.DOScale(1f, (flyTime)/1000f).SetEase(visualConfig.GeneratedScale);
            tr.DOMoveX(targetPos.x, flyTime/1000f).SetEase(visualConfig.GeneratedFlyX);
            tr.DOMoveY(targetPos.y, flyTime/1000f).SetEase(visualConfig.GeneratedFlyX);
            
            spriteRenderer.sortingLayerName = visualConfig.TopLayer;
            spriteRenderer.sortingOrder = 100+sortOrder;
                Debug.Log($"spriteRenderer  >. {spriteRenderer.sortingLayerName }");
            await Task.Delay(flyTime);

            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = visualConfig.NormalLayer;
                spriteRenderer.sortingOrder = 0;
                Debug.Log($"spriteRenderer.sortingLayerName  << {spriteRenderer.sortingLayerName }");
            }
        }
        
        public async Task AnimateDropItem(MergeVisualConfig visualConfig, MergeItemView view, int posX, int posY, Vector2 targetPos, int sortOrder = 0)
        {
            var tr = view.transform;
            var spriteRenderer = view.SpriteRenderer;
            
            tr.localScale = new Vector3(visualConfig.ScaleRatio, visualConfig.ScaleRatio, 1f);
            tr.DOScale(1f, visualConfig.AppearDuraion / 1000f).SetEase(visualConfig.AppearEasing);
            tr.position = new Vector3(posX, posY);
            tr.DOMove(targetPos, 0.5f).SetEase(Ease.OutFlash);
            
            spriteRenderer.sortingLayerName = visualConfig.TopLayer;
            spriteRenderer.sortingOrder = sortOrder;
            await Task.Delay(500);
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = visualConfig.NormalLayer;
                spriteRenderer.sortingOrder = 0;
            }
        }

    }
}