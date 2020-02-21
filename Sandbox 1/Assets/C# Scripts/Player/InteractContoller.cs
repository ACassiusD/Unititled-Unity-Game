using UnityEngine;
using System;
using cakeslice;

public class InteractContoller : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 direction;
    private RaycastHit hitinfo;
    public float distance = 10;
    public float yOffset = -.40f; 
    public bool isDebugging = false;
    Transform hitObject;
    [SerializeField] Material highlightMaterial;
    bool isObjectHit = false;
    int LayerMask; //Layer to ignore
    Outline activeScript;
    float outlineWidth = 0.5f;

    private void Update()
    {
        clearActiveScript();
        isObjectHit = castRay();
        if (isObjectHit)
        {
            CallInteraction(hitinfo);
        }
    }

    //Runs at the beginning of every new frame, removes highlighting and clears the script from memory
    private void clearActiveScript()
    {
        if (activeScript != null)
        {
            activeScript.eraseRenderer = true;
            activeScript = null;
        }
    }

    //Cast ray and return weather a object was hit
    private bool castRay()
    {
        origin = this.transform.position;
        direction = this.transform.TransformDirection(Vector3.forward);
        direction.y += yOffset; //Looks down

        bool isObjectHit = false;
        hitinfo = new RaycastHit();

        //Check if the ray hit something, store it in hitinfo
        if (Physics.Raycast(origin, direction, out hitinfo, distance, 1 << 8))
        {
            isObjectHit = true;
        }
        else
        {
            isObjectHit = false;
        }

        //Debugging
        if (isDebugging)
        {
            debugRay();
        }

        return isObjectHit;
    }

    //If an object intercected with the ray, attempt to call interaction with the hit object
    private void CallInteraction(RaycastHit hitinfo)
    {
        //Get Hit object
        hitObject = hitinfo.transform;
      // Debug.Log(hitObject);
        //If object has no active script, try to find it in the class hierarchy

        if (hitObject.GetComponent<Outline>())
        {
            activeScript = hitObject.GetComponent<Outline>();
        }
        else if (hitObject.parent != null && hitObject.parent.GetComponentInChildren<Outline>() != null)
        {
            activeScript = hitObject.parent.GetComponentInChildren<Outline>();
            hitObject = hitObject.parent;
        }


        if (activeScript != null)
        {
            activeScript.eraseRenderer = false;
          //  Debug.Log("start highlighting");

            //Try to call interact
            if (Input.GetKeyDown("e") || (Input.GetMouseButtonDown(0)))
            {
                try
                {
                    hitObject.SendMessage("interact");
                }
                catch (Exception e)
                {
                    Debug.Log("Object has no interact function");
                }
            }
        }
    }

    private void debugRay()
    {
        Debug.DrawRay(origin, direction * distance, Color.red);
    }
}

