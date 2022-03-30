using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Player
{
    public class PlayerView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _mainAttackEffect;
        [SerializeField] private ParticleSystem _finishAttackEffect;
        private NavMeshAgent _navMeshAgent;

        public Animator GetAnimator() => _animator;
        public ParticleSystem GetMainAttackEffect() => _mainAttackEffect;
        public ParticleSystem GetFinishAttackEffect() => _finishAttackEffect;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

