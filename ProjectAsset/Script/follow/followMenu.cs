using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMenu : MonoBehaviour {

    public GameObject menuCube;
	
	// Update is called once per frame
	void Update () {
        if (menuCube.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        transform.position = menuCube.transform.position;
    }
}
