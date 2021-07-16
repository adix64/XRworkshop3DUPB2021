using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : Fighter
{
    NavMeshAgent agent;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        GetCommonComponents();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = agent.velocity.normalized;
        SetMoveParametersOnAnimator();
        CheckIfGrounded();

        agent.SetDestination(player.position);
        if (agent.remainingDistance < 1.1f)
            animator.SetTrigger("Punch");

    }
    private void OnAnimatorMove()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        ApplyRootRotation(toPlayer);
        agent.velocity = agent.desiredVelocity.normalized *
                         animator.deltaPosition.magnitude / Time.deltaTime;
    }
    private void LateUpdate()
    {
    }
}
