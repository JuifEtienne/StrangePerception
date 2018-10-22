using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class DrawSphere : MonoBehaviour
    {
        public GameObject spherePos;
        public Hand hand;
        public GameObject sphere;
        public GameObject blade;

        GameObject sphereCreated;

        private Vector3 pos;

        bool isCreated = false;

        // Update is called once per frame
        void Update()
        {

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (isCreated)
                {
                    isCreated = false;
                    sphereCreated.GetComponent<SphereCollider>().radius = sphereCreated.transform.localScale.x/2;
                }
                else
                {
                    sphereCreated = Instantiate(sphere) as GameObject;
                    sphereCreated.GetComponent<Renderer>().material = gameObject.GetComponent<SelectionMaterial>().mainMaterial;
                    sphereCreated.transform.parent = transform;
                    sphereCreated.transform.position = spherePos.transform.position;
                    sphereCreated.GetComponent<IsInteractibleSphere>().handsManager = gameObject;
                    sphereCreated.GetComponent<IsInteractibleSphere>().blade = blade;
                    isCreated = true;
                }

            }


            if (isCreated)
            {
                float distance = Vector3.Distance(sphereCreated.transform.position, spherePos.transform.position);
                sphereCreated.transform.localScale = new Vector3(distance, distance, distance);
            }


        }

    }
}