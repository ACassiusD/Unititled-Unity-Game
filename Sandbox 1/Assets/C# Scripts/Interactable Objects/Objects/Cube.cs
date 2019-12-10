using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube : MonoBehaviour, InteractableInterface {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void interact()
    {
        Vector3 temp = new Vector3(7.0f, 0, 0);
        this.transform.position += temp;
    }
}
