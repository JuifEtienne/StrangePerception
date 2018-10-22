using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isInteractableMesh : MonoBehaviour
{

    public GameObject handsManager;
    MeshCollider colliderMesh;
    public GameObject blade;

    private void Start()
    {
        colliderMesh = gameObject.GetComponent<MeshCollider>();
    }



    // Update is called once per frame
    void Update()
    {
        if (handsManager.GetComponent<Interaction>().enabled || blade.activeSelf)
        {
            colliderMesh.enabled = true;
        }
        else
        {
            colliderMesh.enabled = false;
        }

        if (blade.activeSelf)
        {
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

