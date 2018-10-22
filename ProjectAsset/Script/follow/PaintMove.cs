using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintMove : MonoBehaviour
{

    public GameObject contact;

    // Use this for initialization
    void awake()
    {
      
        transform.position = contact.transform.position;
        transform.rotation = contact.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = contact.transform.position;
        transform.rotation = contact.transform.rotation;
    }
}
