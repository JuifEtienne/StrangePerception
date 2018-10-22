using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class isInteractibleBox : MonoBehaviour
{

    public GameObject handsManager;
    BoxCollider colliderMesh;
    public GameObject blade;

    private void Start()
    {
        colliderMesh = gameObject.GetComponent<BoxCollider>();
    }



    // Update is called once per frame
    void Update()
    {
        if (handsManager.GetComponent<Interaction>().enabled)
        {
            colliderMesh.enabled = true;
        }
        else
        {
            colliderMesh.enabled = false;
        }

        if (blade.activeSelf)
        {
            colliderMesh.enabled = true;
            Destroy(GetComponent<Valve.VR.InteractionSystem.Throwable>());
        }
        else
        {
            if(GetComponent<Valve.VR.InteractionSystem.Throwable>() == null)
            {
                gameObject.AddComponent<Valve.VR.InteractionSystem.Throwable>();
            }
            
        }
    }
}
