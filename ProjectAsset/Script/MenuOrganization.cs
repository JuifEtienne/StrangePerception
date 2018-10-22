using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Valve.VR.InteractionSystem
{
    public class MenuOrganization : MonoBehaviour
    {

        public GameObject[] menuItems;
        public GameObject cam;
        public Hand hand;
        public GameObject handsManager;

        bool state;

        List<MonoBehaviour> scripts = new List<MonoBehaviour>();
        List<bool> scriptsBool = new List<bool>();


        private void Awake()
        {
            scripts.Add(handsManager.GetComponent<addLine>());
            scriptsBool.Add(scripts[0].enabled);
            scripts.Add(handsManager.GetComponent<AddScreen>());
            scriptsBool.Add(scripts[1].enabled);
            scripts.Add(handsManager.GetComponent<Interaction>());
            scriptsBool.Add(scripts[2].enabled);
            scripts.Add(handsManager.GetComponent<DrawCube>());
            scriptsBool.Add(scripts[3].enabled);
            scripts.Add(handsManager.GetComponent<DrawSphere>());
            scriptsBool.Add(scripts[4].enabled);
            scripts.Add(handsManager.GetComponent<Cut>());
            scriptsBool.Add(scripts[5].enabled);
            scripts.Add(handsManager.GetComponent<Eraser>());
            scriptsBool.Add(scripts[6].enabled);
            state = false;
        }

        // Update is called once per frame
        void Update()
        {
            /*gameObject.transform.position = cam.transform.position + 1.5f * cam.transform.forward;
            gameObject.transform.LookAt(cam.transform.position, Vector3.up);
            

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                //MENU WAS ON
                if (state)
                {
                    state = false;
                    activateScriptIndex(getIndexActivate());

                    for (i = 0; i < menuItems.Length; ++i)
                    {
                        menuItems[i].SetActive(!menuItems[i].activeSelf); ;
                    }
                }
                //MENU WAS OFF
                else
                {
                    state = true;
                    updateBool();
                    desactivateScript();
                    desactivateObject();

                    for (i = 0; i < menuItems.Length; ++i)
                    {

                        menuItems[i].SetActive(!menuItems[i].activeSelf); ;
                    }
                }
            }


            //The menu is ON and the player press the trigger
            if(state && hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                int index = getIndexActivateObject();
                if(index != -1)
                {
                    state = false;
                    activateScriptIndex(index);
                    for (i = 0; i < menuItems.Length; ++i)
                    {
                        menuItems[i].SetActive(!menuItems[i].activeSelf); ;
                    }
                }
            }*/

        }








        //To check what script is activate
        void updateBool()
        {
            int i;
            for (i = 0; i < scriptsBool.Count; ++i)
            {
                scriptsBool[i] = scripts[i].enabled;
            }
        }

        //To get the index of the script which is active
        int getIndexActivate()
        {
            int i;
            for (i = 0; i < scriptsBool.Count; ++i)
            {
                if (scriptsBool[i])
                {
                    return i;
                }
            }
            return -1;
        }

        //desactivate all of the scripts
        void desactivateScript()
        {
            int i;
            for (i = 0; i < scripts.Count; ++i)
            {
                scripts[i].enabled = false;
            }
        }

        void desactivateObject()
        {
            int i;
            for (i = 0; i < menuItems.Length; ++i)
            {
                menuItems[i].GetComponent<Activate>().activate = false;
            }
        }

        //activate the script i
        void activateScriptIndex(int i)
        {
            if(i <= scripts.Count){
                scripts[i].enabled = true;
            }
        }

        //To know which object was activate by the player
        int getIndexActivateObject()
        {
            int i;
            for (i = 0; i < menuItems.Length; ++i)
            {
                if (menuItems[i].GetComponent<Activate>().activate)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}