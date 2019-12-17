using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target; //What the camera looks at
    [SerializeField]
    private float currentDistance = -40;
    [SerializeField]
    private float desiredCameraDistance = -40;
    [SerializeField]
    private float currentHeight = 0;
    public int cameraZoomSpeed = 5;
    public float maxZDistanceFromTarget;
    public float minZDistanceFromTarget;
    public float maxCameraHeight = 50;
    public float minCameraHeight = 0; 

    // Use this for initialization
    void Start () {
        //Set negative to be behind the player
        maxZDistanceFromTarget = 100;
        minZDistanceFromTarget = 5;

    }
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 newCameraLocation = new Vector3();
        float scrollBarInput = Input.GetAxis("Mouse ScrollWheel");

        //This was easier to write with all the numbers being positive
        float posDesiredDistance = desiredCameraDistance * -1; 

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
