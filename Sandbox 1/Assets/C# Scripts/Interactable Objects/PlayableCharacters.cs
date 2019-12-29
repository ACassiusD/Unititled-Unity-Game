//Extendable class that contains core logic for playable characters such as collisions and movement.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacters : MonoBehaviour {

	public CharacterController controller = new CharacterController();
	public int speed = 30;
	private Vector3 moveDirection;
	public float jumpForce = 50f;
	public float gravityScale = .25f;
	public int numOfJumps = 0;
	public int maxJumps = 2;
    public int rotationSpeed = 3;
    public bool isBeingControlled = false;

    //Should be moved to a different class
    public bool mountAi = false;
    Random random;
    public bool isTurning = false;
    public bool isWalking = false;
    public float turnLength = 0.0f;
    public float walkLength = 0.0f;
    public int turnDirection = 0;
    float halfSecondLength = 0.5f;
    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;


    void Start () {
        onCreate();
        random = new Random();
        controller = GetComponent<CharacterController>();
        if (!isBeingControlled)
        {
            mountAi = true;
        }
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    //Update is called once per frame
    void Update()
    {
         moveCharacterController();
    }

	//Control player with character controller
	protected void moveCharacterController(){

        
        if (isBeingControlled)
        {
            getPlayerInput();
        }
        else if(mountAi) //AI Controlling the movement controller Should be moved to a different class
        {
            getMountAiInput();
        }
        else
        {
            //Zero out all movement except verticle momentum
            moveDirection = new Vector3(0, moveDirection.y, 0);
        }

        //Calculate gravity
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);

        //Transform objects direction in global space
        moveDirection = transform.TransformDirection(moveDirection);

        //Apply vector to object
        controller.Move(moveDirection * Time.deltaTime);
    }

    //Apply the input to the players movement
    void getPlayerInput()
    {

            //Check if the player is grounded and reset the jumps
            if (controller.isGrounded)
            {
                numOfJumps = 0;
            }

            //Apply y velocity from last frame and apply the forward/backward movement
            moveDirection = new Vector3(0, moveDirection.y, Input.GetAxis("Vertical") * speed);

            //If jump is pressed, add a force to the verticle axis
            if (controller.isGrounded || (!controller.isGrounded && numOfJumps < maxJumps))
            {
                if (Input.GetButtonDown("Jump"))
                {
                    numOfJumps = numOfJumps + 1;
                    moveDirection.y = jumpForce;
                }
                else
                {
                    if (controller.isGrounded)
                    {
                        moveDirection.y = 0;
                    }
                }
            }

            //Apply player rotation 
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
    }

    void getMountAiInput()
    {
        moveDirection = new Vector3(0, 0, 0);

        //Generate random value to see if the mount should turn or walk forward
        if (!isTurning)
        {
            float max = int.MaxValue;
            float randnum = Random.Range(0f, max);
            double percentage = System.Math.Round((((double)randnum / (double)max) * 100), 1);

            if (percentage > 98d)
            {
                isTurning = true;
                turnDirection = Random.Range(-1, 1);
                turnLength = halfSecondLength * Random.Range(0, 3);
            }
        }
        else
        {
            turnLength -= Time.deltaTime;
            if (turnLength < 0)
            {
                turnDirection = 0;
                turnLength = 0;
                isTurning = false;
            }
        }

        transform.Rotate(0, turnDirection * aiRotationSpeed, 0);

        //Do the same for walking
        if (!isWalking)
        {
            int max = int.MaxValue;
            int randnum = Random.Range(0, max);
            double percentage = System.Math.Round((((double)randnum / (double)max) * 100), 1);

            if (percentage > 99.5d)
            {
                isWalking = true;
                walkLength = halfSecondLength * Random.Range(0, 4);
            }
        }
        else
        {
            walkLength -= Time.deltaTime;
            if (walkLength < 0)
            {
                walkLength = 0;
                isWalking = false;
            }
        }

        if (isTurning)
        {
            transform.Rotate(0, turnDirection * aiRotationSpeed, 0);
        }

        if (isWalking)
        {
            moveDirection = new Vector3(0, moveDirection.y, 1 * aiMoveSpeed);
        }
    }
}
