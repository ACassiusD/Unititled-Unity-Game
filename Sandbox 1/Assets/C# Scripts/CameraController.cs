using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target; //Assign the taget that camera will look at 
    public float currentHeight = 0;
    public float maxCameraHeight = 30; //How high above the target the camera should be
    public float minCameraHeight = 5;
    public float maxCameraDistance = 120;
    public float minCameraDistance = 15;
    public float currentCameraDistance = -40; //Initial distance from player
    public float desiredCameraDistance = -40; //Desired Distance from player
    public int cameraZoomSpeed = 5;
    Vector3 newCameraLocation; //Holds the new position of the camera for this frame
    public float dist = 0;

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
        //Zoom in
        if (scrollInput > 0f && dist > minCameraDistance)
        {
            desiredCameraDistance += cameraZoomSpeed;
        }
        //Zoom out
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && dist < maxCameraDistance) // backwards
        {
            desiredCameraDistance -= cameraZoomSpeed;
        }

        //Calculate the new camera position by lerping the current distance from play to the desired distance from player
        
        dist = Vector3.Distance(this.transform.localPosition, target.transform.localPosition);

        if (dist > maxCameraDistance)
        {
            dist = maxCameraDistance;
        }
        else if(dist < minCameraDistance)
        {
            dist = minCameraDistance;
        }
        
        float positionLimit = maxCameraDistance - minCameraDistance;
        float distanceposition = dist - minCameraDistance;
        float distancePercentage = distanceposition / positionLimit;

       
        currentHeight = Mathf.Lerp(minCameraHeight, maxCameraHeight, distancePercentage);


        //Set new camera location to target position
        newCameraLocation = target.localPosition;


        currentCameraDistance = Mathf.Lerp(currentCameraDistance, desiredCameraDistance, Time.deltaTime * cameraZoomSpeed);
        //Setting cameras position to players position (+/-) players direction (the cameras offset)
        //Add the height and camera distance
        newCameraLocation = newCameraLocation + target.transform.TransformDirection(new Vector3(0, currentHeight, currentCameraDistance));

        float futureDist = Vector3.Distance(newCameraLocation, target.transform.localPosition);
        Debug.Log(newCameraLocation);
        Debug.Log(futureDist);
        if (futureDist > maxCameraDistance)
        {
            newCameraLocation = transform.position;
            desiredCameraDistance = maxCameraDistance * -1;
            //desiredCameraDistance = currentCameraDistance;
        }
        else if (futureDist < minCameraDistance)
        {
            newCameraLocation = transform.position;
            desiredCameraDistance = minCameraDistance;
            //desiredCameraDistance = currentCameraDistance;
        }

        //Finally, move the camera
        transform.position = newCameraLocation;
        
        //Rotate the camera to "look" at the player	
        transform.LookAt(target);
    }
}
