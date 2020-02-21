using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dounut : MonoBehaviour, InteractableInterface
{
    public Vector3 startPosition;
    public Vector3 endPositon;
    int framePosition = 0;
    int frameOfAnimation = 0;
    public string direction;
    Quaternion startR;
    Quaternion endR;
    int verticalMovement = 78; //The height for the "elevator" type interaction


    // Use this for initialization
    void Start()
    {
        direction = "none"; //Current movement direction of the dounut
        frameOfAnimation = 60 * 5;
        startPosition = this.transform.localPosition;
        endPositon = startPosition;
        endPositon.y = endPositon.y + verticalMovement;
        startR = this.transform.rotation;
        endR = transform.rotation * Quaternion.AngleAxis(-170f, Vector3.forward);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(direction != "none")
        {
            switch (direction)
            {
                case "up":
                    framePosition++;
                    break;
                case "down":
                    framePosition--;
                    break;
                default:
                    break;
            }

            if (framePosition > frameOfAnimation)
            {
                framePosition = frameOfAnimation;
                direction = "none";
            }
            else if (framePosition < 0)
            {
                framePosition = 0;
                direction = "none";
            }

            float percentage = ((float)framePosition / (float)frameOfAnimation);
           // Debug.Log(this.transform.localPosition); // Calculating Di
           // Debug.Log(this.transform.localRotation); // Calculating Di
           // Debug.Log(this); // Calculating Di

            
            transform.localPosition = Vector3.Lerp(startPosition, endPositon, percentage);
            transform.rotation = Quaternion.Lerp(startR, endR, percentage);
        }
    }


    public void interact()
    {


        if (direction == "none")
        {
            if (this.transform.localPosition == startPosition)
            {
                direction = "up";
            }else if (this.transform.localPosition == endPositon)
            {
                direction = "down";
            }
        }
    }
}
