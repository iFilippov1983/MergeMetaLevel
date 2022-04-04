using Data;
using Game;
using UnityEngine;

namespace Enemy
{
    public class EnemyView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _burnMaterial;
        [SerializeField] private GameObject _model;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _appearEffect;
        [SerializeField] private ParticleSystem _mainAttackEffect;
        [SerializeField] private ParticleSystem _secondaryAttackEffect;
        [SerializeField] private ParticleSystem _finishAttackEffect;
        [SerializeField] private ParticleSystem _gotHitEffect;
        [SerializeField] private ParticleSystem _deathEffect;

        public EnemyType EnemyType => _type;
        public Animator GetAnimator() => _animator;
        public Material DefaultMaterial => _defaultMaterial;
        public Material BurnMaterial => _burnMaterial;        public GameObject Model => _model;
        public ParticleSystem GetAppearEffect() => _appearEffect;
        public ParticleSystem GetMainAttackEffect() => _mainAttackEffect;
        public ParticleSystem GetSecondaryAttackEffect() => _secondaryAttackEffect;
        public ParticleSystem GetFinishAttackEffect() => _finishAttackEffect;
        public ParticleSystem GetGotHitEffect() => _gotHitEffect;
        public ParticleSystem GetDeathEffect() => _deathEffect;
    }
}

