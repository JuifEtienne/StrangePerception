using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class addLine : MonoBehaviour
    {
        public GameObject paintSurface;
        public GameObject lineSelection;
        public Hand hand;
        public GameObject blade;

        private List<Vector3> Points = new List<Vector3>();
        private float timer = 0f;
        private Vector3 pos;
        private int numOfLine = -1;
        private LineRenderer[] lrs;

        // Update is called once per frame
        void Update()
        {

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                createLine(paintSurface);
            }

           
            if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                timer = 0f;
                Points.Clear();
                lrs[numOfLine].GetComponent<LineColliderInstanciate>().enabled = false;
                lrs[numOfLine].GetComponent<LineColliderInstanciate>().enabled = true;

            }


            if (timer != 0f)
            {
                timer += Time.deltaTime;
                //pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                //Know where the player has clicked --- REPLACE BY THE POSITION OF THE VR CONTROLLER
                Vector3 pos = hand.transform.position + -0.02f * hand.transform.up + 0.05f * hand.transform.forward; ;

                Points.Add(pos);
                changeLine();

                //Points.Add(new Vector3(pos.x, pos.y, 0f));
                //Debug.Log(Input.mousePosition);
            }
         

        }

        void changeLine()
        {
            lrs[numOfLine].positionCount = Points.ToArray().Length - 1;
            lrs[numOfLine].SetPositions(Points.ToArray());

        }

        void createLine(GameObject obj)
        {
            timer += Time.deltaTime;

            //Creation of a new Line
            
            GameObject go;
            go = Instantiate(obj) as GameObject;
            go.transform.parent = transform;
            go.GetComponent<isInteractibleLine>().handsManager = gameObject;
            go.GetComponent<isInteractibleLine>().blade = blade;
            go.GetComponent<Renderer>().material = gameObject.GetComponent<SelectionMaterial>().mainMaterial;
            go.GetComponent<LineRenderer>().material = gameObject.GetComponent<SelectionMaterial>().mainMaterial;

            //go.GetComponent<LineRenderer>.SetWidth
            //Take all of the lineRenderer
            lrs = GetComponentsInChildren<LineRenderer>();
            numOfLine = lrs.Length - 1;
        }


        private void Awake()
        {
            lrs = GetComponentsInChildren<LineRenderer>();
        }

        
    }
}