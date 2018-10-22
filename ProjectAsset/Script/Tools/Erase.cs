using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erase : MonoBehaviour {
    public GameObject HandsManager;
    public Valve.VR.InteractionSystem.Hand hand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cutable" && HandsManager.GetComponent<Eraser>().enabled && hand.controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Destroy(other.gameObject);

        }
    }
}
