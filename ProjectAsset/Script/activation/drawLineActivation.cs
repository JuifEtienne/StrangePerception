using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawLineActivation : MonoBehaviour
{
    public GameObject handsManager;
    public GameObject sphere;
    public GameObject canvas;
    public Valve.VR.InteractionSystem.Hand hand;

    public bool activation;

    void Update()
    {
        if (activation)
        {
            handsManager.GetComponent<Organizate>().state = false;
            canvas.SetActive(false);
            sphere.SetActive(true);
            handsManager.GetComponent<Valve.VR.InteractionSystem.addLine>().enabled = true;
            activation = false;
        } 
    }

}
