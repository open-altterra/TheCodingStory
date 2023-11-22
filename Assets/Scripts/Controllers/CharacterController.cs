using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    public bool LockRaycast;
    public bool LockRaycastFromBanner;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = agent.transform.position;
    }
    public void StopCharacter()
    {
        agent.SetDestination(agent.transform.position);
    }

    private void Update()
    {
        if (!LockRaycastFromBanner)
            if (!LockRaycast)
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        agent.SetDestination(hit.point);
                        if (!animator.IsInTransition(0))
                            animator.CrossFade("Walk", 0.05f);
                    }
                }

        if (Vector3.Distance(agent.transform.position, lastPosition) / Time.deltaTime < 0.25f)
        {
            if (!animator.GetNextAnimatorStateInfo(0).IsName("Idle"))
            {
                if (!animator.IsInTransition(0))
                    animator.CrossFade("Idle", 0.25f);
            }
        }
        else
        {
            //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            if (!animator.GetNextAnimatorStateInfo(0).IsName("Walk"))
            {
                if (!animator.IsInTransition(0))
                    animator.CrossFade("Walk", 0.1f);
            }
        }

        lastPosition = agent.transform.position;
    }

}
