using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isInteractibleLine : MonoBehaviour {

    public GameObject handsManager;
    public BoxCollider[] box;
    public GameObject blade;


    // Update is called once per frame
    void Update () {
        if (handsManager.GetComponent<Interaction>().enabled || blade.activeSelf)
        {
            for(int i = 0; i < box.Length; ++i)
            {
                box[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < box.Length; ++i)
            {
                box[i].enabled = false;
            }

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
