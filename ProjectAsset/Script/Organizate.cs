using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organizate : MonoBehaviour {

    public GameObject canvas;
    public GameObject cam;
    public Valve.VR.InteractionSystem.Hand hand;
    public GameObject sphere;

    public GameObject laser;

    public bool state;

    private void Awake()
    {
        canvas.SetActive(false);
        hand.GetComponent<VRUIINPUT>().enabled = false;
        hand.GetComponent<SteamVR_LaserPointer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        


        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            //MENU WAS ON
            if (!state)
            {
                state = true;
                sphere.SetActive(false);
                gameObject.GetComponent<Valve.VR.InteractionSystem.addLine>().enabled = false;
                gameObject.GetComponent<Valve.VR.InteractionSystem.AddScreen>().enabled = false;
                gameObject.GetComponent<Interaction>().enabled = false;
                gameObject.GetComponent<Valve.VR.InteractionSystem.DrawCube>().enabled = false;
                gameObject.GetComponent<Valve.VR.InteractionSystem.DrawSphere>().enabled = false;
                gameObject.GetComponent<Cut>().enabled = false;
                gameObject.GetComponent<Eraser>().enabled = false;
            }
        }

        if (state)
        {
            canvas.SetActive(true);
            canvas.transform.position = cam.transform.position + 10f * cam.transform.forward;
            canvas.transform.LookAt(cam.transform.position, cam.transform.up);
            hand.GetComponent<VRUIINPUT>().enabled = true;
            hand.GetComponent<SteamVR_LaserPointer>().enabled = true;
            if(laser != null)
            {
                laser.SetActive(true);
            }
            
        }
        else
        {
            canvas.SetActive(false);
            hand.GetComponent<VRUIINPUT>().enabled = false;
            hand.GetComponent<SteamVR_LaserPointer>().enabled = false;
            if (laser != null)
            {
                laser.SetActive(false); 
            }
        }
    }
}
