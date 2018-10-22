using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut : MonoBehaviour {
    public GameObject blade;

    private void OnEnable()
    {
        blade.SetActive(true);    
    }

    private void OnDisable()
    {
        blade.SetActive(false);   
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
