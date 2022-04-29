using System;
using UnityEngine;

//Player movement component contains a state machine, a working group states relevent to the player, varaible, and functions relevent to player movement.
//It is the "Brain" of player movement.
public class PlayerMovementComponent : MoveComponent
{
    public IdleState standing;
    public DuckingState ducking;
    public JumpingState jumping;
    public RidingState riding;
    public FallingState falling;
    public MovingState moving;
    public EmoteState emote;
    public Transform cam;
    //Only movement variables specific to player should go here
    public bool isRiding = false;
    public Mount activeMount; //This might need to be moved out
    public float groundCheckDistance = 0.6f;
    public float groundCheckOutwardOffset = 0.22f;
    public float groundCheckVerticleOffset = 0f;
    public float groundCheckTime = 0.1f;
    public float groundCheckTimer = 0f; //How long after jumping can we check for isGrounded again.

    private void Start()
    {
        //Debug.Log("Jump state time = " + this.jumpStateTime);
        //distanceToGround = this.GetComponent<Collider>().bounds.extents.y; 
        activeMount = null;
        isBeingControlled = true;
        //Initialize the players states.
        standing = new IdleState(stateMachine, this);
        jumping = new JumpingState(stateMachine, this);
        riding = new RidingState(stateMachine, this);
        falling = new FallingState(stateMachine, this);
        moving = new MovingState(stateMachine, this);
        emote = new EmoteState(stateMachine, this);


        if (characterController.isGrounded)
            stateMachine.Initialize(standing); 
        else
            stateMachine.Initialize(jumping); 
    }

    public void RegenerateStamina()
    {
        if(sprintTimer < 0)
            sprintTimer = 0; 
        
        if(sprintTimer < sprintLimit)
            sprintTimer += 1*Time.deltaTime;
        
        if(sprintTimer > sprintLimit)
            sprintTimer = sprintLimit;

        playerScript.UpdateStaminaUI();
    }

    public void MoveToMountedPosition() //Moves entity to mounted seating position
    {
        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + this.activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }


    public void SetActiveMount(Mount mount)    //This should not be in the move controller.
    {
        this.activeMount = mount;
        isRiding = true;
    }

    public void MovePlayerViaInput()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

       // Debug.Log(horizontal + vertical);

        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
            //Mathf.Atan2(direction.x, direction.z) - Gives us the angle in radians our player needs to turn
            //Mathf.Rad2Deg Update the angle to degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //Smooth the angle to not immediatly point towards it.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);
            //Rotate our player on the Y axis by the angle 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //reference for more information - https://www.youtube.com/watch?v=4HpC--2iowE
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            //Debug.Log(characterController.isGrounded);
        }
        else
        {
            isMoving = false;
        }
    }

    public bool isGrounded()
    {
        var currentPos = transform.position;
        var frontOffsetPosition = transform.position + (transform.forward * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var backOffsetPosition = transform.position + (-transform.forward * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var leftOffsetPosition = transform.position + (-transform.right * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var rightOffsetPosition = transform.position + (transform.right * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        if (isDebugging)
        {
            Debug.DrawRay(frontOffsetPosition, (Vector3.forward * groundCheckDistance), Color.red);
            Debug.DrawRay(backOffsetPosition, (-Vector3.forward * groundCheckDistance), Color.blue);
            Debug.DrawRay(leftOffsetPosition, (-Vector3.right * groundCheckDistance), Color.yellow);
            Debug.DrawRay(rightOffsetPosition, (Vector3.right * groundCheckDistance), Color.magenta);
            Debug.DrawRay(currentPos, (-Vector3.up), Color.green);
        }

        if (characterController.isGrounded)
        {
            return true;
            Debug.Log("IS GROUNDED");
        }
        else //Fallback check for slopes using a ray
        {
            if (Physics.Raycast(currentPos, -Vector3.up, groundCheckDistance))
            {
                // Debug.Log("IS GROUNDED");
                return true;
            }
            else if(Physics.Raycast(frontOffsetPosition, -Vector3.up, groundCheckDistance))
            {
                return true;
            }
            else if (Physics.Raycast(backOffsetPosition, -Vector3.up, groundCheckDistance))
            {
                return true;
            }
            else if (Physics.Raycast(leftOffsetPosition, -Vector3.up, groundCheckDistance))
            {
                return true;
            }
            else if (Physics.Raycast(rightOffsetPosition, -Vector3.up, groundCheckDistance))
            {
                return true;
            }
            else
            {
                // Debug.Log("NOT IS GROUNDED");
                return false;
            }
        }
    }
}
