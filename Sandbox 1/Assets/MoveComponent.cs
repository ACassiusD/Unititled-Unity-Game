using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract MoveComponent class, with basic commands that most entities would want.
public abstract class MoveComponent : MonoBehaviour
{
    public static BetaCharacter playerScript;
    //public static Transform playerTransform;
    public StateMachine stateMachine;
    public CharacterController characterController;
    Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded
    public float moveSpeed = 50f;
    public int jumpCount = 0;
    public int maxJumps = 2;
    public float jumpHeight = 20;
    public float gravity = -8f;
    public float turnSmoothTime = 0.1f;
    public bool isBeingControlled = false;
    public bool isControllable = true;
    public float rotationSpeed;
    public bool isMoving = false;
    public Transform target;
    public float distanceFromTarget;
    public float minDistanceFromTarget = 20;
    public bool isRunning = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        stateMachine = new StateMachine();
    }

    protected void Update()
    {
        isMoving = checkIfMoving();
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
    }

    public void AddJumpVelocity() //Add jump Velocity to Y axis
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpCount++;
    }


    public void ZeroYVelocityIfGrounded() //Grounded reset velocity check
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }


    public void addGravity()
    {
        velocity.y += gravity * Time.deltaTime; //Gravity Check
    }


    public void MidAirJump()
    {
        if (!characterController.isGrounded && (jumpCount < maxJumps)) //Check for double jump
        {
            AddJumpVelocity();
        }
    }


    public void AddVelocityAndMove()
    {
        characterController.Move(velocity * Time.deltaTime);
    }


    public void ResetMoveParams()
    {
    }

    public bool checkIfMoving()
    {
        var inputV = Input.GetAxis("Vertical");
        var inputH = Input.GetAxis("Horizontal");
        if (inputV > 0 || inputV < 0 || inputH > 0 || inputH < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    //Should be moved to entity script
    public void MoveTowardsTarget()
    {
        this.transform.LookAt(this.target.transform);
        distanceFromTarget = Vector3.Distance(this.target.transform.position, this.transform.position);

        //Check if enemy is close enough to the player
        if (distanceFromTarget <= minDistanceFromTarget)
        {
            if (isMoving != false)
            {
                isMoving = false;
            }
            return;
        }

        if (isMoving == false)
        {
            isMoving = true;
        }

        float step = moveSpeed * Time.deltaTime; // calculate distance to move





        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        //if (direction.magnitude >= 0.1f)
        //{
        //    //Mathf.Atan2(direction.x, direction.z) - Gives us the angle in radians our player needs to turn (foreward, back, left, right)
        //    //Mathf.Rad2Deg Update the angle to degrees
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //    //Smooth the angle to not immediatly point towards it.
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);
        //    //Rotate our player on the Y axis by the angle 
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //    //reference for more information - https://www.youtube.com/watch?v=4HpC--2iowE
        //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        //    characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        //}
    }

    public float getDistanceFromTarget()
    {
        distanceFromTarget = Vector3.Distance(this.target.position, characterController.transform.position);
        return distanceFromTarget;
    }

    public bool IsInRangeOfPlayer()
    {
        if (getDistanceFromTarget() < 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public BetaCharacter getPlayerScript()
    {
        if(playerScript == null)
        {
            playerScript = PlayerManager.Instance.getPlayerScript();
        }
        return playerScript;
    }
}
