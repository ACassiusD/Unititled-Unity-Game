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
    
    void Start () {
        onCreate();
        controller = GetComponent<CharacterController>();
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
}
