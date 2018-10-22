using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInteractibleSphere : MonoBehaviour {

    public GameObject handsManager;
    SphereCollider colliderMesh;
    public GameObject blade;

    private void Start()
    {
        colliderMesh = gameObject.GetComponent<SphereCollider>();
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
            if (GetComponent<Valve.VR.InteractionSystem.Throwable>() == null)
            {
                gameObject.AddComponent<Valve.VR.InteractionSystem.Throwable>();
            }

        }
    }
}
