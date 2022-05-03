using UnityEngine;
using UnityEngine.AI;
using Game;
using Tool;
using UnityEngine.Animations.Rigging;
using static UnityEngine.ParticleSystem;

namespace Player
{
    public class PlayerView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _popupPrefabName;
        [SerializeField] private Transform _popupSpawnPoint;
        [SerializeField] private ParticleSystem _levelUpParticle;
        [SerializeField] private Transform _weapon;
        [SerializeField] private MultiAimConstraint _headAim;
        [SerializeField] private Transform _headAimTarget;
        [SerializeField] private FlyParticleSystem _flyParticleSystem;

        private NavMeshAgent _navMeshAgent;
        private GameObject _popupPrefab;

        public FlyParticleSystem FlyParticleSystem => _flyParticleSystem;
        public Transform HeadAimTarget => _headAimTarget;
        public MultiAimConstraint HeadAim => _headAim;
        public Transform Weapon => _weapon;
        public Animator GetAnimator() => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Transform PopupSpawnPoint => _popupSpawnPoint;
        public ParticleSystem LevelUpParticle => _levelUpParticle;
        public GameObject PopupPrefab
        {
            get
            {
                if (_popupPrefab == null) _popupPrefab =
                             Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _popupPrefabName));
                return _popupPrefab;
            }
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

