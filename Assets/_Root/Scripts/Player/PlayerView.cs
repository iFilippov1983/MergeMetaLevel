using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Player
{
    public class PlayerView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _appearEffect;
        [SerializeField] private ParticleSystem _mainAttackEffect;
        [SerializeField] private ParticleSystem _secondaryAttackEffect;
        [SerializeField] private ParticleSystem _finishAttackEffect;
        [SerializeField] private ParticleSystem _gotHitEffect;
        [SerializeField] private ParticleSystem _deathEffect;
        private NavMeshAgent _navMeshAgent;

        public Animator GetAnimator() => _animator;
        public ParticleSystem GetAppearEffect() => _appearEffect; 
        public ParticleSystem GetMainAttackEffect() => _mainAttackEffect;
        public ParticleSystem GetSecondaryAttackEffect() => _secondaryAttackEffect;
        public ParticleSystem GetFinishAttackEffect() => _finishAttackEffect;
        public ParticleSystem GetGotHitEffect() => _gotHitEffect;
        public ParticleSystem GetDeathEffect() => _deathEffect;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

