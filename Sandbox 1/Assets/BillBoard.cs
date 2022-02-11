using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform cam;
    public GameObject target;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
        //var wantedPos = Camera.main.WorldToScreenPoint(target.transform.position);
        //var spiderHP = GameObject.Find("SpiderHP");
        //spiderHP.GetComponent<RectTransform>().position = wantedPos;
    }
}
