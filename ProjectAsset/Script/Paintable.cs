using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour {

    public GameObject brush;
    public float BrushSize = 0.1f;
    public Transform target;
    int paintableMask;
    float camRayLength = 100f;

    // Use this for initialization
    void Awake () {
        paintableMask = LayerMask.GetMask("Paintable");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit, camRayLength, paintableMask))
            {
                //var go = Instantiate(brush, hit.point + Vector3.forward * 0.1f, transform.rotation);
                var go2 = Instantiate(brush, hit.point - Vector3.forward * 0.1f, Quaternion.Inverse(transform.rotation));
                //go.transform.localScale = Vector3.one * BrushSize;
                go2.transform.localScale = Vector3.one * BrushSize;
            }
        }
	}
}
