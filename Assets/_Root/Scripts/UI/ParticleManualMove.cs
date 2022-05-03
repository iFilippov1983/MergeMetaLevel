using PathCreation.Examples;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameUI
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleManualMove : MonoBehaviour
    {
        [SerializeField] private float _force = 20f;
        [SerializeField] private bool _doFly;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _groupPoint;
        [SerializeField] private GeneratePathBehaviour _pathBeh;
        [SerializeField] private PathCreation.PathCreator _pathCreator;
        private Transform _start;
        private Transform _target;


        private Transform _thisTransform;
        private Vector3 _defaultScale;
        //private Vector3 _targetScale;
        private ParticleSystem.Particle[] _particles;
        private ParticleSystem.MainModule _mainModule;
        private int _maxParticles;

        public Action OnParticleTargetCollision;


        public void SetPathPoints(Transform start, Transform target)
        {
            _pathBeh.waypoints[0] = start;
            _pathBeh.waypoints[1] = target;
            _pathBeh.enabled = false;
        }

        public async void DoFly()
        {
            _doFly = true;
            await Task.Delay(500);
            _pathBeh.enabled = true;
        }

        private void Awake()
        {
            _thisTransform = transform;
            _defaultScale = transform.localScale;
            //_targetScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        private void Start() 
        {
            _mainModule = _particleSystem.main;
            _maxParticles = _mainModule.maxParticles;

            _doFly = false;
        }

        private void LateUpdate()
        {
            if (_doFly)
            {
                if (_particles == null || _particles.Length < _maxParticles)
                {
                    _particles = new ParticleSystem.Particle[_maxParticles];
                    HandleGravityScale(false);
                }

                _particleSystem.GetParticles(_particles);

                float forceDeltaTime = _force * Time.deltaTime;
                Vector3 targetTransformedPosition;

                switch (_mainModule.simulationSpace)
                {
                    case ParticleSystemSimulationSpace.Local:
                        targetTransformedPosition = transform.InverseTransformPoint(_groupPoint.position);
                        break;
                    case ParticleSystemSimulationSpace.World:
                        targetTransformedPosition = _groupPoint.position;
                        break;
                    case ParticleSystemSimulationSpace.Custom:
                        targetTransformedPosition = _mainModule.customSimulationSpace.InverseTransformPoint(_groupPoint.position);
                        break;
                    default:
                        throw new System.NotSupportedException
                            (
                            string.Format("Unsupported simulation space '{0}'.",
                            Enum.GetName(typeof(ParticleSystemSimulationSpace), _mainModule.simulationSpace))
                            );
                }

                for (int i = 0; i < _particles.Length; i++)
                {
                    Vector3 directionToTarget = Vector3.Normalize(targetTransformedPosition - _particles[i].position);
                    Vector3 seekForce = directionToTarget * forceDeltaTime;
                    _particles[i].velocity += seekForce;
                }
                _particleSystem.SetParticles(_particles, _particles.Length);
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.layer.Equals(LayerMask.NameToLayer(Data.Literal.LayerName_Target)))
            {
                //OnParticleTargetCollision?.Invoke();
                Debug.Log($"collision {LayerMask.LayerToName(other.layer)}");
            }
        }

        private void OnDisable()
        {
            HandleGravityScale(true);
            _thisTransform.localScale = _defaultScale;
        } 

        private void HandleGravityScale(bool isEnabled)
        {
            var mainModule = _particleSystem.main;
            mainModule.gravityModifier = isEnabled ? 2f : 0f;
            _doFly = !isEnabled;
        }
    }
}

