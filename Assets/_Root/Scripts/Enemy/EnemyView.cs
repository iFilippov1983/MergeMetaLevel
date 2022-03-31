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
        [SerializeField] private ParticleSystem _mainAttackEffect;
        [SerializeField] private ParticleSystem _finishAttackEffect;

        public EnemyType EnemyType => _type;
        public Animator GetAnimator() => _animator;
        public ParticleSystem GetMainAttackEffect() => _mainAttackEffect;
        public ParticleSystem GetFinishAttackEffect() => _finishAttackEffect;
    }
}

