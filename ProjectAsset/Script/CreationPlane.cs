using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class CreationPlane : MonoBehaviour
    {
        public Hand hand;

        private void Start()
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                
                gameObject.GetComponent<MeshCollider>().enabled = true;
            }


            if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                
                gameObject.GetComponent<MeshCollider>().enabled = false;
            }


        }

    }
}