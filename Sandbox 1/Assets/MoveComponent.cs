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
    public Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded
    public float moveSpeed = 50f; // Active speed
    public float walkSpeed = 20; //Intended walk speed
    public float runSpeed = 100; //Intended run speed
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
    public bool inHitStun = false;
    public int knockBackForce = 0;
    public Vector3 knockBackDirection;
    public float stunTimer = 0f;
    public bool isEnabled = true;
    public int enterChaseDistance = 100;
    public int exitChaseDistance = 160;
    public int attackRange = 10;
    public bool isDebugging = false;
    public float jumpStateTime = 1f;
    public float sprintTimer = 0f;
    public float sprintLimit = 3f;
    public bool requiresStamina = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        stateMachine = new StateMachine();
        updateMoveSpeed();
        ResetSprintTimer();
    }

    protected void Update()
    {
        if (isEnabled)
        {
            toggleRun();
            UpdateSprintTimer();
        }
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
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    public void addGravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime; //Gravity Check
        }
    }

    public void MidAirJump()
    {
        if (!IsGrounded() && (jumpCount < maxJumps)) //Check for double jump
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

    public float getDistanceFromTarget()
    {
        if(this.target == null) { return 0f; }
        return Vector3.Distance(this.target.position, characterController.transform.position);
    }
    public bool InChaseRange()
    {
        if (getDistanceFromTarget() < exitChaseDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool InAttackRange()
    {
        if (getDistanceFromTarget() <= minDistanceFromTarget)
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

    public void Knockback()
    {
        Vector3 direction;
        if(knockBackDirection.magnitude > 0.1)
        {
            direction = knockBackDirection; 
        }
        else
        {
            direction = this.transform.forward * -1; 
        }

        AddJumpVelocity(); //TODO: Make this work with the knockback variable passed from Recieve damage to have a variable knockback amount.


        this.knockBackDirection = Vector3.zero;
        this.knockBackForce = 0;
    }

    public void toggleRun(bool force = false)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || force)
        {
            if(stateMachine.CurrentState.ToString() != "FallingState" && stateMachine.CurrentState.ToString() != "JumpingState")
            {
                if (isRunning ? isRunning = false : isRunning = true) ;
                updateMoveSpeed();
            }
        }
    }
    public void updateMoveSpeed()
    {

        if (isRunning)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

   protected void UpdateSprintTimer()
   {
        if (requiresStamina)
        {
            if (!isRunning || !isBeingControlled) { return; }
            sprintTimer -= Time.deltaTime;

            if (sprintTimer <= 0)
            {
                if (isDebugging)
                {
                    Debug.Log("Sprint Ended");
                }

                toggleRun(true);
            }
            else
            {
                if (isDebugging)
                {
                    Debug.Log("Sprint Timer = " + sprintTimer);
                }
            }
        }
   }
   protected void ResetSprintTimer()
   {
        sprintTimer = sprintLimit;
   }

    public virtual bool IsGrounded()
    {
        return characterController.isGrounded;
    }
}
