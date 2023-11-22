using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
