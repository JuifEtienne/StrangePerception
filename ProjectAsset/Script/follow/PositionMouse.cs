using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMouse : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        
	}
}
