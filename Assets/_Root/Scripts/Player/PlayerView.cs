using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public Animator Animator => _animator;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

