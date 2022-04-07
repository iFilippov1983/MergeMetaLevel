using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Features.Fx
{
    public class RewardFlyBatch : MonoBehaviour
    {
        public Transform Target;
        public List<RewardFly> Batch;

        [Button]
        void TestFly()
            => Fly(transform.position, Target.position, () => { });

        private void Awake()
        {
            foreach (var fx in Batch)
                fx.gameObject.SetActive(true);
        }

        public async void Fly(Vector3 from, Vector3 to, Action cbOnComplete)
        {
            transform.position = from;
            
            var target = to;
            foreach (var fx in Batch.Take(Random.Range(5, Batch.Count+1)))
            {
                fx.FlyTo(target);
                await Task.Delay(fx.Config.DelayPerFx.ToMs());
            }
            
            await Task.Delay(3000);
            cbOnComplete?.Invoke();
            // Fly();
        }
    }
}