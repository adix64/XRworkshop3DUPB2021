using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : Fighter
{
    public Transform opponent;
    // Start is called before the first frame update
    void Start()
    {
        GetCommonComponents();
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        moveDirection = GetMoveDirection();
        SetMoveParametersOnAnimator();
        HandleJump();
        HandleAttack();
        HandleFallOffPlatform();
    }

    private void HandleFallOffPlatform()
    {
        if (transform.position.y < -30f)
            transform.position = initPos;
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("Punch");
    }

    private void OnAnimatorMove()
    {
        ApplyRootMotion();
        Vector3 lookDirection = moveDirection; // by default, personajul se uita in directia deplasarii
        Vector3 toOpponent = opponent.position - transform.position; //directia de la personaj catre oponent
        Vector3 toOpponentXoZ = toOpponent;//...>
        float distToEnemy = toOpponentXoZ.magnitude;
        animator.SetFloat("distToEnemy", distToEnemy);
        capsule.radius = 0.3f + (1f - Mathf.Clamp01((distToEnemy - 3f) / 2f)) * 0.15f;
        toOpponentXoZ.y = 0f; // aceeasi directie, dar in plan orizontal
        if (toOpponent.magnitude < 4f) //se uita catre oponent doar daca e la mai putin de 4 metri de acesta
            lookDirection = toOpponentXoZ.normalized;
        ApplyRootRotation(lookDirection);
    }
    
    private void ApplyRootMotion()
    {
        if (!grounded)
            return;
        //transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;//pentru non-rigidbody
        float velY = rigidbody.velocity.y;
        //rigidbody.velocity = moveDirection * moveSpeed; //viteza constanta deplasare smooth
        rigidbody.velocity = animator.deltaPosition / Time.deltaTime; // viteza de root motion data de animator
        rigidbody.velocity += Vector3.up * velY;
    }

    private void HandleJump()
    {
        CheckIfGrounded();
        animator.SetBool("Grounded", grounded);

        if (grounded && Input.GetButtonUp("Jump"))
        {
            Vector3 jumpDir = (Vector3.up + moveDirection).normalized;
            Debug.DrawLine(transform.position, transform.position + jumpDir * jumpPower, Color.cyan, 1f);
            rigidbody.AddForce(jumpDir * jumpPower, ForceMode.VelocityChange);
        }
    }

  
    private Vector3 GetMoveDirection()
    {
        float moveX = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru D, 0 altfel
        float moveZ = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru W, 0 altfel

        Vector3 moveDirection = camera.forward * moveZ + camera.right * moveX;
        moveDirection.y = 0f; // miscare in plan XoZ(orizontal)
        moveDirection = moveDirection.normalized; //ca sa mentina aceeasi viteza inainte/inapoi/stanga/dreapta/diagonala
        return moveDirection;
    }
}
