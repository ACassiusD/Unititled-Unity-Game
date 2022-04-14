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

    //Needs to be refactored to consider more than just the player, also should not just return true/false based on if the input is pressed, check if player is actually moving or update isMoving in the state.
    public bool checkIfMoving()
    {
        return this.isMoving;
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    //Should be moved to entity script
    public void MoveTowardsTarget()
    {
        Vector3 directionTowardsPlayer = (target.transform.position - this.transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionTowardsPlayer.x, directionTowardsPlayer.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //this.transform.LookAt(this.target.transform);
        distanceFromTarget = Vector3.Distance(this.target.transform.position, this.transform.position);

        //Check if enemy is close enough to the player
        if (distanceFromTarget >= minDistanceFromTarget)
        {
            this.isMoving = true;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            this.isMoving = false;
        }
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
