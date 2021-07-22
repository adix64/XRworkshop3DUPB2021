using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDisplay : MonoBehaviour
{
    public GameObject F, B, L, R;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // -1 pentru tasta A, 1 pentru D, 0 altfel
        float moveZ = Input.GetAxis("Vertical"); // -1 pentru tasta S, 1 pentru W, 0 altfel

        F.SetActive(moveZ > 0f);
        B.SetActive(moveZ < 0f);
        R.SetActive(moveX > 0f);
        L.SetActive(moveX < 0f);
    }
}
