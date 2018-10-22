using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class lineActivate : MonoBehaviour {

    public GameObject handsManager;
    public Valve.VR.InteractionSystem.Hand hand;

    bool wasActivate = false;
    
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
                Debug.Log("The menu for Line is working");
                
            }

        }
    }

    private void OnEnable()
    {
        wasActivate = handsManager.GetComponent<addLine>().isActiveAndEnabled;
    }
}
