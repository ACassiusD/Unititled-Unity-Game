using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cam;

    private void Awake()
    {
        cam = GameObject.Find("MainCamera1").GetComponent<Camera>().transform;
        if (cam == null){Debug.LogError("Missing Reference to camera in Billboard.cs for " + this.name);}
    }
    void Update()
    {
        //transform.forward = cam.forward * -1;
        transform.forward = cam.forward;
        //transform.LookAt(transform.position + cam.forward);
    }
}
