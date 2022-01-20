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
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    //Update is called once per frame
    void Update()
    {
        MoveCharacterController();
    }

	//Control player with character controller
	protected void MoveCharacterController(){
        isWalking = isMoving();

        if (isBeingControlled)
        {
            Debug.Log(this.gameObject.name + "s speed == " + speed);
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

    public bool isMoving()
    {
        var input = Input.GetAxis("Vertical");
        if ( input > 0 || input < 0)
        {
            return true;
        }
        else
        {
             return false;
        }
        
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
