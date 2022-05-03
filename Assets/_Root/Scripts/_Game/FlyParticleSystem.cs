using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [HideMonoScript]
    public class FlyParticleSystem : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _pullPoint;
        [SerializeField] private ParticleSystem _particleSystem;
        private readonly List<ParticleSystem.Particle> _enteredParticles = new List<ParticleSystem.Particle>();
        private Transform _defaultParent;
        private Vector3 _defaultPos;
        private const int _valueToSentInCollisionEvent = 1;

        public Action<int> OnParticleCollision;

        private void Start()
        {
            AddParticleTrigger(_collider);

            _defaultParent = _particleSystem.transform.parent;
            _defaultPos = _particleSystem.transform.localPosition;
        }

        private void OnParticleTrigger()
        {
            _enteredParticles.Clear();

            var triggerParticlesCount =
                _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enteredParticles);


            for (var i = 0; i < triggerParticlesCount; i++)
            {
                //Debug.Log("action on particle enter trigger");
                OnParticleCollision?.Invoke(_valueToSentInCollisionEvent);
            }
                
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void AddParticleTrigger(Collider collider)
        {
            _particleSystem.trigger.AddCollider(collider);
        }

        [Button]
        public void Emit(int count)
        {
            StartCoroutine(Pull(count));
        }

        private float CreateParticles(int count)
        {
            var burst = _particleSystem.emission.GetBurst(0);
            
            burst.cycleCount = 1;
            burst.minCount = (short) count;
            burst.maxCount = (short) count;

            _particleSystem.emission.SetBurst(0, burst);
            _particleSystem.Play();

            return burst.repeatInterval * burst.cycleCount;
        }

        private IEnumerator Pull(int count)
        {
            _particleSystem.transform.parent = _defaultParent;
            _particleSystem.transform.localPosition = _defaultPos;

            var createTime = CreateParticles(count);

            yield return new WaitForSeconds(createTime + 0.3f);
            yield return new WaitForEndOfFrame();

            _particleSystem.transform.parent = _pullPoint.transform;
            _particleSystem.transform.localPosition = Vector3.zero;
        }
    }
}