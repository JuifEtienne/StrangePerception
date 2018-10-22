using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour {

    public Valve.VR.InteractionSystem.Hand hand;

    private void Update()
    {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (gameObject.GetComponent<drawLineActivation>() != null) {

                gameObject.GetComponent<drawLineActivation>().activation = true;

            }
            else if (gameObject.GetComponent<captureActivation>() != null)
            {

                gameObject.GetComponent<captureActivation>().activation = true;

            }
            else if (gameObject.GetComponent<interactionActivation>() != null)
            {

                gameObject.GetComponent<interactionActivation>().activation = true;

            }
            else if (gameObject.GetComponent<cubeActivation>() != null)
            {

                gameObject.GetComponent<cubeActivation>().activation = true;

            }
            else if (gameObject.GetComponent<sphereActivation>() != null)
            {

                gameObject.GetComponent<sphereActivation>().activation = true;

            }
            else if (gameObject.GetComponent<cutActivation>() != null)
            {

                gameObject.GetComponent<cutActivation>().activation = true;

            }
            else if (gameObject.GetComponent<eraseActivation>() != null)
            {

                gameObject.GetComponent<eraseActivation>().activation = true;

            }

        }
    }
}
