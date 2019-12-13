using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	
    float currentHeight = 0;
    Vector3 newCameraLocation; //Holds the new position of the camera for this frame
    public Transform target; //Assign the taget that camera will look at 
    public int cameraZoomSpeed = 5;
    public float cameraDistance = -40; //Initial distance from player
    public float desiredCameraDistance = -40; //Desired Distance from player
    public float maxZDistanceFromTarget;
    public float minZDistanceFromTarget;
    public float maxCameraHeight = 30; //How high above the target the camera should be
    public float minCameraHeight = 5;


    // Use this for initialization
    void Start () {
        maxZDistanceFromTarget = 100;
        minZDistanceFromTarget = 5;

    }
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
    }

    void MoveCamera()
    {
        //Get scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");


        if((desiredCameraDistance * -1) >= minZDistanceFromTarget && (desiredCameraDistance * -1) <= maxZDistanceFromTarget) {
            //Get Zoom input and adjust desired camera distance
            if (scrollInput > 0f && (desiredCameraDistance * -1) > minZDistanceFromTarget)
            {
                desiredCameraDistance += cameraZoomSpeed;
            }
            //Zoom out
            else if (scrollInput < 0f && (desiredCameraDistance * -1) < maxZDistanceFromTarget) // backwards
            {
                desiredCameraDistance -= cameraZoomSpeed;
            }
        }




        //if (desiredCameraDistance < (maxZDistanceFromTarget * -1))
        //{
        //    desiredCameraDistance = maxZDistanceFromTarget;
        //}
        //else if (desiredCameraDistance > (minZDistanceFromTarget * -1))
        //{
        //    desiredCameraDistance = minCameraDistance;
        //}



        ////Calculate the new camera position by lerping the current distance from play to the desired distance from player

        //dist = Vector3.Distance(this.transform.localPosition, target.transform.localPosition);

        //if (dist > maxCameraDistance)
        //{
        //    dist = maxCameraDistance;
        //}
        //else if (dist < minCameraDistance)
        //{
        //    dist = minCameraDistance;
        //}

        //float positionLimit = maxCameraDistance - minCameraDistance;
        //float distanceposition = dist - minCameraDistance;
        //float distancePercentage = distanceposition / positionLimit;


        ////currentHeight = Mathf.Lerp(minCameraHeight, maxCameraHeight, distancePercentage);


        ////Set new camera location to target position
        //newCameraLocation = target.localPosition;


        cameraDistance = Mathf.Lerp(cameraDistance, desiredCameraDistance, Time.deltaTime * cameraZoomSpeed);

        ////Setting cameras position to players position (+/-) players direction (the cameras offset)


        //float futureDist = Vector3.Distance(newCameraLocation, target.transform.localPosition);
        ////Debug.Log(newCameraLocation);
        ////Debug.Log(futureDist);

        //Start by setting initial new camera position to the player position
        newCameraLocation = target.localPosition;

        //Add height y axis offset, and distance in the z axis offset
        newCameraLocation = newCameraLocation + target.transform.TransformDirection(new Vector3(0, currentHeight, cameraDistance));

        //Finally, move the camera
        transform.position = newCameraLocation;
        
        //Rotate the camera to "look" at the player	
        transform.LookAt(target);
    }
}
