using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMaterial : MonoBehaviour {
    public Material[] materials;
    public Material mainMaterial;
    public GameObject sphere;
    public Valve.VR.InteractionSystem.Hand hand;

    int currentIndex = 0;

	// Use this for initialization
	void Start () {
        mainMaterial = materials[currentIndex];
	}
	
	// Update is called once per frame
	void Update () {
        sphere.GetComponent<Renderer>().material = gameObject.GetComponent<SelectionMaterial>().mainMaterial;

        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.x > 0.7f)
            {
                nextOneRight();
                mainMaterial = materials[currentIndex];
            }

            else if (touchpad.x < -0.7f)
            {
                nextOneLeft();
                mainMaterial = materials[currentIndex];
            }

        }
    }

    void nextOneLeft()
    {
        if(currentIndex == 0)
        {
            currentIndex = materials.Length - 1;
        }
        else
        {
            --currentIndex;
        }
    }

    void nextOneRight()
    {
        if (currentIndex == materials.Length)
        {
            currentIndex = 0;
        }
        else
        {
            ++currentIndex;
        }
    }
}
