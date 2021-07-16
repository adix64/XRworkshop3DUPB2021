using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fighter : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotSpeed = 3f;
    public float jumpPower = 6f;
    public float groundedThreshold = .15f;
    protected Rigidbody rigidbody;
    protected CapsuleCollider capsule;
    protected Animator animator;
    public Transform camera;
    protected Vector3 moveDirection;
    protected Vector3 initPos;
    protected bool grounded;
    protected AnimatorStateInfo stateInfo;
    // Start is called before the first frame update
    protected void GetCommonComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
    }
    protected void CheckIfGrounded()
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;

        grounded = false;
        float maxDistance = groundedThreshold;

        //aruncam 9 raze in jos, din centrul si de pe conturul(cercul) capsulei ca sa detectam pamant sub picioare
        for (float xOffset = -1f; xOffset <= 1f; xOffset += 1f)
        {
            for (float zOffset = -1f; zOffset <= 1f; zOffset += 1f)
            {
                Vector3 capsuleContourOffset = new Vector3(xOffset, 0f, zOffset).normalized * capsule.radius;
                ray.origin = transform.position + Vector3.up * groundedThreshold / 2f + capsuleContourOffset;
                if (Physics.Raycast(ray, maxDistance))
                {//handle ray intersection, personajul e pe pamant
                    grounded = true;
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.green);
                }
                else
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.red);
            }
        }
    }
    protected void SetMoveParametersOnAnimator()
    {
        Vector3 characterSpaceMoveDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat("Forward", characterSpaceMoveDirection.z, 0.2f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDirection.x, 0.2f, Time.deltaTime);
    }
    protected void ApplyRootRotation(Vector3 lookDirection)
    {
        if (moveDirection.magnitude < 10e-3f || stateInfo.IsTag("punch"))
            return;
        //interpolam orientarea personajului catre directia in care vrem sa se uite
        Quaternion newRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSpeed);
    }

}
