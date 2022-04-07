using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configs.Meta;
using Configs.Quests;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Components.Rewards
{
    public class RewardMono : MonoBehaviour
    {
        public ResourcesConfig ResourcesConfig;
        public Image Icon;
        public TextMeshProUGUI Count;
        
        public Vector2 offset;
        public float Duration = 0.6f;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0,0,1,1);
        public float IdleDuration = 0.6f;
        public GameObject[] ActivateOnEnd;

        private List<RewardMono> _created = new List<RewardMono>();
        private Resource _resource;

        [Button]
        public async void TEST_SHOW()
        {
            Show(new List<Resource>(){Resource.Coins, Resource.Hearts, Resource.Coins});
        }

        public async void Show(List<Resource> rewards)
        {
            Clear();
            
            var rewardsCount = rewards.Count;
            for (int i = 0; i < rewardsCount; i++)
            {
                var reward = rewards[i];
                CloneShow(reward, i, rewardsCount-1);
                await Task.Delay(200);
            }
        }
        
        public void Clear()
        {
            _created.ForEach(v => Destroy(v.gameObject));
            _created.Clear();
        }

        public void CloneShow(Resource resource, int index, int maxIndex)
        {
            // var clone = Instantiate(this, RootCtx.tmpInstance.Ui.View.transform, true);
            var clone = Instantiate(this, transform.position, Quaternion.identity, CoreRoot.tmpInstance.Ui.View.transform);
            clone.ShowOne(resource, index, maxIndex);
            // _created.Add(clone);
        }
        
        private async void ShowOne(Resource resource, int index, int maxIndex)
        {
            gameObject.SetActive(true);
            UpdateView(resource);
            await DoMove(index, maxIndex);
            Destroy(gameObject);
        }

        private async Task DoMove(int index, int maxIndex)
        {
            var tr = transform;
            var ratio = (float)index / maxIndex;
            var posX = Mathf.Lerp(-offset.x, offset.x, ratio);
            var posY = offset.y;

            // tr.localPosition = Vector3.zero;
            tr.DOMoveX(tr.position.x + posX, Duration).SetEase(Curve);
            tr.DOMoveY(tr.position.y + posY, Duration).SetEase(Curve);

            await Task.Delay(Duration.ToMs());
            await Task.Delay(IdleDuration.ToMs());

            var resourceView = _resource.FindResourceView(CoreRoot.tmpInstance);
            if (resourceView != null)
                resourceView.SetCountOffset(_resource.Count);  
            
            // Wait QuestUi Hide
            await Task.Delay(1100);
            
            Count.DOColor(new Color(1, 1, 1, 0), Duration/3f);
            var flyTarget = _resource.FindFlyTarget(CoreRoot.tmpInstance);
            Debug.Log($"flyTarget  [{flyTarget.x}] [{flyTarget.y}");
            tr.DOMoveX(flyTarget.x, 0.8f).SetEase(Ease.OutCubic);
            tr.DOMoveY(flyTarget.y, 0.8f).SetEase(Ease.Linear);
            await Task.Delay(800);
            
            if (resourceView != null)
                resourceView.TweenCountOffset();  
            
            foreach (var go in ActivateOnEnd)
                go.SetActive(true);
            await Task.Delay(400);
        }

        private void UpdateView(Resource resource)
        {
            _resource = resource;
            Count.text = resource.CountStr();
            Icon.sprite = ResourcesConfig.GetImage(resource.Type);
        }
    }
}