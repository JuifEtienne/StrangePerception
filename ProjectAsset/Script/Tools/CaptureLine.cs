using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class CaptureLine : MonoBehaviour
    {

        public LineRenderer points;
        public Hand hand;
        //public CopyLineAndTrian poly;
        public Material selection;
        public Material transpa;

        private List<Vector3> Points = new List<Vector3>();
        private float timer = 0f;
        private Vector3 pos;
        int load;
        //bool capture = false;

        private void Start()
        {
            load = 0;
        }

        private void Update()
        {
            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                points.material = selection;
                points.positionCount = 0;
                points.SetPositions(new Vector3[0]);
                timer += Time.deltaTime;
            }
            if (timer != 0f)
            {
                timer += Time.deltaTime;
                ++load;
                //Know where the player has clicked --- REPLACE BY THE POSITION OF THE VR CONTROLLER

                if (load % 5 == 0)
                {
                    Points.Add(hand.transform.position);
                    changeLine();
                }
            }
            //REPLACE MOUSE BY LEFT AND RIGHT CONTROLLER BACK BUTTON
            if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("End");
                timer = 0f;
                //poly.capture = true;
                Points.Clear();
                //points.material = transpa;
            }

        }

        void changeLine()
        {
            points.positionCount = Points.ToArray().Length - 1;
            points.SetPositions(Points.ToArray());

        }

    }
}
