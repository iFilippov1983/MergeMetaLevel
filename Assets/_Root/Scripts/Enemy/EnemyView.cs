using Data;
using Game;
using Sirenix.OdinInspector;
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

        public EnemyType EnemyType => _type;
        public Animator GetAnimator() => _animator;
        public Material DefaultMaterial => _defaultMaterial;
        public Material BurnMaterial => _burnMaterial;        
        public GameObject Model => _model;
    }
}

