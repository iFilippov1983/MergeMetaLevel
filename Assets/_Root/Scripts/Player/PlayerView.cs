using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform _movePositionTransform;
    private NavMeshAgent _navMeshAgent;

    public Action<Collider> OnTrigger;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other);
    }
}