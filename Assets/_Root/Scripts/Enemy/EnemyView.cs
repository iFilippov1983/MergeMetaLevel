using Data;
using Game;

using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public EnemyType EnemyType => _type;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Animator GetAnimator() => _animator;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

