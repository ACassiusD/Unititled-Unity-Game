using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target; //Assign the taget that camera will look at 
    public float cameraHeight = 20; //How high above the target the camera should be
    public float currentCameraDistance = -40; //Initial distance from player
    public float desiredCameraDistance = -40; //Desired Distance from player
    public int cameraZoomSpeed = 5;
    Vector3 newCameraLocation; //Holds the new position of the camera for this frame

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
    }

    void MoveCamera()
    {
        //Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel"); 

        //Check scroll input what clicked, Increase or decrease the desired camera distance
        if (scrollInput > 0f)
        {
            desiredCameraDistance += cameraZoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            desiredCameraDistance -= cameraZoomSpeed;
        }

        //Calculate the new camera position by lerping the current distance from play to the desired distance from player
        currentCameraDistance = Mathf.Lerp(currentCameraDistance, desiredCameraDistance, Time.deltaTime * cameraZoomSpeed);

        //Set new camera location to target position
        newCameraLocation = target.localPosition;

        //Setting cameras position to players position (+/-) players direction (the cameras offset)
        //Add the height and camera distance
        newCameraLocation = newCameraLocation + target.transform.TransformDirection(new Vector3(0, cameraHeight, currentCameraDistance));
        
        //Finally, move the camera
        transform.position = newCameraLocation;
        
        //Rotate the camera to "look" at the player	
        transform.LookAt(target);
    }
}
