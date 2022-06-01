using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cam;

    void Update()
    {
        //transform.forward = cam.forward * -1;
        transform.forward = cam.forward;
        //transform.LookAt(transform.position + cam.forward);
    }
}
