using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWith : MonoBehaviour {
    bool maintain = false;
    GameObject follow;

    private void Update()
    {
        if (maintain)
        {
            transform.position = follow.transform.position + 0.5f * follow.transform.forward;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            maintain = false;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Yes");
        if (Input.GetKeyDown(KeyCode.P))
        {
            maintain = true;
            follow = collision.collider.gameObject;
        }
        
        
    }
}
