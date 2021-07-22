using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float yaw = 0f;
    float pitch = 0f;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public Transform player;
    public Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = player.position + transform.TransformDirection(cameraOffset);
    }
}
