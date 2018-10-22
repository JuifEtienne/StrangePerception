using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour {
    public GameObject blade;

    private void OnEnable()
    {
        blade.SetActive(true);
        blade.GetComponent<Cutable>().enabled = false;
        blade.GetComponent<Erase>().enabled = true;
        blade.GetComponent<MeshCollider>().enabled = true;
    }

    private void OnDisable()
    {
        blade.SetActive(false);
        blade.GetComponent<Cutable>().enabled = true;
        blade.GetComponent<Erase>().enabled = false;
        blade.GetComponent<MeshCollider>().enabled = false;
    }


    

    

   

    // Update is called once per frame
    void Update () {
        
        }

}
