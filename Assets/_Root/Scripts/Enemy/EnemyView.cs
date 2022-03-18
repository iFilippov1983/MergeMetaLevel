using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Animation _animation;
        [SerializeField] private List<EnemyAnimation> _animations;
        private NavMeshAgent _navMeshAgent;

        public EnemyType EnemyType => _type;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Animation Animation => _animation;
        public List<EnemyAnimation> AnimationsList => _animations;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

