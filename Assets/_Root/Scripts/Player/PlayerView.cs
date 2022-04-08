using UnityEngine;
using UnityEngine.AI;
using Game;
using Tool;

namespace Player
{
    public class PlayerView : MonoBehaviour, IAnimatorHolder
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _popupPrefabName;

        private NavMeshAgent _navMeshAgent;
        private GameObject _popupPrefab;

        public Animator GetAnimator() => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
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

