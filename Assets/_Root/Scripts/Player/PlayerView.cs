using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Player
{
    public class PlayerView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private Animator _animator;

        private NavMeshAgent _navMeshAgent;

        public Animator GetAnimator() => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

