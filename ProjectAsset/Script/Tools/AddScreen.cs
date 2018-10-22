using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class AddScreen : MonoBehaviour
    {
        public Hand hand;
        public GameObject Screen;
        public Material rightEyeImage;
        public GameObject blade;

        // Update is called once per frame
        void Update()
        {

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GameObject go;
                go = Instantiate(Screen) as GameObject;
                go.GetComponent<isInteractableMesh>().handsManager = gameObject;
                go.GetComponent<isInteractableMesh>().blade = blade;
                go.transform.position = hand.transform.position;
                Quaternion rotate = hand.transform.rotation;
                go.transform.rotation = new Quaternion(rotate.x, rotate.y, rotate.z, rotate.w) ;
                //go.transform.rotation = Quaternion.AngleAxis(180, transform.up);

                Texture2D finalEye = (rightEyeImage.mainTexture as Texture2D);
                int width = finalEye.width;
                int height = finalEye.height;

                Texture2D planeTex = new Texture2D(width, height, TextureFormat.RGB24, false);
                /*for(int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        planeTex.SetPixel(i, j, finalEye.GetPixel(i, j));
                    }
                }*/

                Texture2D texCopy = new Texture2D(finalEye.width, finalEye.height, finalEye.format, finalEye.mipmapCount > 1);
                for(int i=0; i < texCopy.width; ++i)
                {
                    for (int j = 0; j < texCopy.height; ++j)
                    {
                        texCopy.SetPixel(i, j, new Color(texCopy.GetPixel(i, j).r, texCopy.GetPixel(i, j).g, texCopy.GetPixel(i, j).b, 1f));
                    }
                }
                texCopy.LoadRawTextureData(finalEye.GetRawTextureData());
                texCopy.Apply();

                go.GetComponent<MeshRenderer>().material.mainTexture = texCopy;
            }

        }
    }
}