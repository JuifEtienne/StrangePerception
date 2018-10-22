using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMove : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void awake () {
        Vector3 pos = player.transform.position;
        pos += 23.9f * player.transform.forward;
        pos.y = 0;
        transform.position = pos;
        transform.rotation = player.transform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = player.transform.position;
        pos += 23.9f * player.transform.forward;
        pos.y = 0;
        transform.position = pos;
        transform.rotation = player.transform.rotation * Quaternion.AngleAxis(-90, Vector3.right);
    }
}
