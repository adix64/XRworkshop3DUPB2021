using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float moveSpeed = 1f;
    Rigidbody rigidbody;
    public Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru D, 0 altfel
        float moveZ = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru W, 0 altfel

        Vector3 moveDirection = camera.forward * moveZ + camera.right * moveX;
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;
        //transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;//pentru non-rigidbody
        float velY = rigidbody.velocity.y;
        rigidbody.velocity = moveDirection * moveSpeed;
        rigidbody.velocity += Vector3.up * velY;
    }
    private void FixedUpdate()
    {
        
    }
}
