using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float currentDistance = -90;
    [SerializeField]
    private float desiredCameraDistance = -90;
    [SerializeField]
    private float currentHeight = 0;
    [SerializeField]
    bool isBeingControlled;
    public int cameraZoomSpeed = 5;
    public float maxZDistanceFromTarget;
    public float minZDistanceFromTarget;
    public float maxCameraHeight = 75;
    public float minCameraHeight = 0;

    public Transform target; //What the camera looks at


    // Use this for initialization
    void Start () {
        //Set negative to be behind the player
        maxZDistanceFromTarget = 200;
        minZDistanceFromTarget = 5;
        isBeingControlled = false;
    }
	
	// Update is called once per frame
	void Update () {
        isBeingControlled = false;
        if (Input.GetKey("c"))
        {
            Debug.Log("hit");
            isBeingControlled = true;
        }

        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 newCameraLocation = new Vector3();
        float scrollBarInput = Input.GetAxis("Mouse ScrollWheel");

        //This was easier to write with all the numbers being positive
        float posDesiredDistance = desiredCameraDistance * -1;

        if (isBeingControlled)
        {
            //Zoom In
            if (scrollBarInput > 0f && posDesiredDistance > minZDistanceFromTarget)
            {
                desiredCameraDistance += cameraZoomSpeed;
            }
            //Zoom out
            else if (scrollBarInput < 0f && posDesiredDistance < maxZDistanceFromTarget) // backwards
            {
                desiredCameraDistance -= cameraZoomSpeed;
            }
        }

        //Get camera height
        currentDistance = Mathf.Lerp(currentDistance, desiredCameraDistance, Time.deltaTime * cameraZoomSpeed);
        float posCurrentDistance = currentDistance * -1;
        float difference = maxZDistanceFromTarget - minZDistanceFromTarget;
        float percentageThroughDifference = (posCurrentDistance-5) / difference;
        float cameraHeight = Mathf.Lerp(minCameraHeight, maxCameraHeight, percentageThroughDifference);
        currentHeight = cameraHeight;

        //Start by setting initial new camera position to the player position
        newCameraLocation = target.localPosition;

        //Add height y axis offset, and distance in the z axis offset
        newCameraLocation = newCameraLocation + target.transform.TransformDirection(new Vector3(0, currentHeight, currentDistance));

        //Finally, move the camera
        transform.position = newCameraLocation;
        
        //Rotate the camera to "look" at the player	
        transform.LookAt(target);
    }
}
