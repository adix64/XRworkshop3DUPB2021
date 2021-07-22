using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : Fighter
{
    NavMeshAgent agent;
    public Transform player;
    public bool offensive = false;
    float timeActive = 0f;
    // Start is called before the first frame update
    void Start()
    {
        GetCommonComponents();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(OffensiveCoroutine(4));
        animator.speed = 0.6f;
    }

    IEnumerator OffensiveCoroutine(float t)
    {
        yield return new WaitForSeconds(t);
        offensive = Random.Range(0, 3) == 0;
        yield return StartCoroutine(OffensiveCoroutine(Random.Range(0.5f, 2.5f)));
    }

    // Update is called once per frame
    void Update()
    {
        timeActive += Time.deltaTime;
        moveDirection = agent.velocity.normalized;
        SetMoveParametersOnAnimator();
        CheckIfGrounded();

        agent.SetDestination(player.position);
        HandleAttack();

    }

    private void HandleAttack()
    {
        if (!offensive)
            return;
        if (agent.remainingDistance < 1.1f)
            animator.SetTrigger("Punch");
    }

    private void OnAnimatorMove()
    {
        if (timeActive < 2f)
            return;
        Vector3 toPlayer = (player.position - transform.position);
        float distToEnemy = toPlayer.magnitude;
        animator.SetFloat("distToEnemy", distToEnemy);
        capsule.radius = 0.3f + (1f - Mathf.Clamp01((distToEnemy - 3f) / 2f)) * 0.15f;
        ApplyRootRotation(toPlayer.normalized);
        agent.velocity = agent.desiredVelocity.normalized *
                         animator.deltaPosition.magnitude / Time.deltaTime;
    }
    private void LateUpdate()
    {
    }
}
