using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public EnemyType EnemyType => _type;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public Animator Animator => _animator;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}

