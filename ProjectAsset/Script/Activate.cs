using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour {

    public Valve.VR.InteractionSystem.Hand hand;
    public bool activate = false;

    // Update is called once per frame
    void Update()
    {
        //transform.position = cam.transform.position + 0.4f * cam.transform.forward - 0.1f * cam.transform.right;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "hand")
        {
            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                activate = true;
            }

        }
    }
}
