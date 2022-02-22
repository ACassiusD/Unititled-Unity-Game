using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player movement component contains a state machine, a working group states relevent to the player, varaible, and functions relevent to player movement.
//It is the "Brain" of player movement.
public class PlayerMovementComponent : MoveComponent
{
    public StandingState standing;
    public DuckingState ducking;
    public JumpingState jumping;
    public RidingState riding;
    public int jumpCount = 0;
    public CharacterController characterController;
    public float turnSmoothTime = 0.1f;
    public Transform cam;
    public int maxJumps = 2;
    public float jumpHeight = 20;
    public float gravity = -8f;
    public bool isBeingControlled = true;
    public bool isControllable = true;
    public bool isRiding = false;
    public Mount activeMount;
    public float rotationSpeed;
    Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded.


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        standing = new StandingState(movementSM, this);
        ducking = new DuckingState(movementSM, this);
        jumping = new JumpingState(movementSM, this);
        riding = new RidingState(movementSM, this);

        if (characterController.isGrounded)
        {
            movementSM.Initialize(standing);
        }
        else
        {
            movementSM.Initialize(jumping);
        }
        base.Start();

    }
    private void Update()
    {
        base.Update();

    }

    //Moves entity to mounted seating position
    public void MoveToMountedPosition()
    {
        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + this.activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }

    public void setActiveMount(Mount mount)
    {
        this.activeMount = mount;
        isRiding = true;
    }

    public void MoveByInput()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        if (direction.magnitude >= 0.1f)
        {
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
        }
    }

    //Add jump Velocity to Y axis
    public void AddJumpVelocity()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpCount++;
    }

    //Grounded reset velocity check
    public void ZeroYVelocityIfGrounded()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    public void addGravity()
    {
        //Gravity Check
        velocity.y += gravity * Time.deltaTime;
    }
    
    public void MidAirJump()
    {
        //Check for double jump
        if (!characterController.isGrounded && (jumpCount < maxJumps))
        {
            AddJumpVelocity();
        }
    }

    public void AddVelocityAndMove()
    {
        characterController.Move(velocity * Time.deltaTime);
    }

}
