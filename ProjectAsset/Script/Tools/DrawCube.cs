using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class DrawCube : MonoBehaviour
    {
        public GameObject sphere;
        public Hand hand;
        public GameObject cube;
        public GameObject blade;

        GameObject cubeCreated;

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
                    //cubeCreated.GetComponent<BoxCollider>().size = cubeCreated.transform.localScale;
                }
                else
                {
                    cubeCreated = Instantiate(cube) as GameObject;
                    cubeCreated.GetComponent<Renderer>().material = gameObject.GetComponent<SelectionMaterial>().mainMaterial;
                    cubeCreated.transform.parent = transform;
                    cubeCreated.transform.position = sphere.transform.position;
                    cubeCreated.GetComponent<isInteractibleBox>().handsManager = gameObject;
                    cubeCreated.GetComponent<isInteractibleBox>().blade = blade;
                    isCreated = true;
                }
                
            }


            if (isCreated)
            {
                float distance = Vector3.Distance(cubeCreated.transform.position, sphere.transform.position);
                cubeCreated.transform.localScale = new Vector3(distance, distance, distance);
            }


        }
                        
    }
}