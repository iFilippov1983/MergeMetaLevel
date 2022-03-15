using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerView : MonoBehaviour
{
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;

    public Action<Collider> OnTrigger;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Transform Transform => _transform;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other);
    }
}
