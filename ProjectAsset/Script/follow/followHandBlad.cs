using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class followHandBlad : MonoBehaviour
    {
        public Hand hand;
        float up = -0.02f;
        float forward = 0.05f;

        // Update is called once per frame
        void Update()
        {

            transform.position = hand.transform.position;
            Quaternion rotate = hand.transform.rotation;
            transform.rotation = new Quaternion(rotate.x, rotate.y, rotate.z, rotate.w);
            transform.position += up * hand.transform.up + forward * hand.transform.forward;
            transform.position += 0.2f*transform.forward;
        }
    }
}