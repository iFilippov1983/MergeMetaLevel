using Data;
using Game;
using Tool;
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
        [SerializeField] private string _popupPrefabName;

        private GameObject _popupPrefab;

        public EnemyType EnemyType => _type;
        public Animator GetAnimator() => _animator;
        public Material DefaultMaterial => _defaultMaterial;
        public Material BurnMaterial => _burnMaterial;
        public GameObject Model => _model;
        public ParticleSystem AppearEffect => _appearEffect;
        public GameObject PopupPrefab
        {
            get
            {
                if (_popupPrefab == null) _popupPrefab =
                         Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _popupPrefabName));
                return _popupPrefab;
            }
        }
    }
}

