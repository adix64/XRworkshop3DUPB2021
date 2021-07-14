using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotSpeed = 3f;
    public float jumpPower = 6f;
    public float groundedThreshold = .15f;
    Rigidbody rigidbody;
    CapsuleCollider capsule;
    Animator animator;
    public Transform camera;
    Vector3 moveDirection;
    Vector3 initPos;
    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = GetMoveDirection();
        SetAnimatorParameters();
        ApplyRootRotation();
        HandleJump();
        if (transform.position.y < -30f)
            transform.position = initPos;
    }
    private void OnAnimatorMove()
    {
        ApplyRootMotion();
        
    }
    private void SetAnimatorParameters()
    {
        Vector3 characterSpaceMoveDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat("Forward", characterSpaceMoveDirection.z, 0.2f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDirection.x, 0.2f, Time.deltaTime);
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
        Ray ray = new Ray();
        
        ray.direction = Vector3.down;

        grounded = false;
        float maxDistance = groundedThreshold;
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

        if (grounded)
        {
            if (Input.GetButtonUp("Jump"))
            {
                Vector3 jumpDir = (Vector3.up + moveDirection).normalized;
                Debug.DrawLine(transform.position, transform.position + jumpDir * jumpPower, Color.cyan, 1f);
                rigidbody.AddForce(jumpDir * jumpPower, ForceMode.VelocityChange);
            }
        }
        animator.SetBool("Grounded", grounded);
    }

    private void ApplyRootRotation()
    {
        if (moveDirection.magnitude < 10e-3f)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotSpeed);


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
