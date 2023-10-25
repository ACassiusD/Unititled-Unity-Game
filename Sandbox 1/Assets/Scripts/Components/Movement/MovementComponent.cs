using UnityEngine;

//Base/Abstract MoveComponent class, contains movement properties and methods to manipulate those properties
[RequireComponent(typeof(CharacterController))]
public abstract class MovementComponent : MonoBehaviour
{
    public static PlayerEntity playerScript;
    //public static Transform playerTransform;
    public StateMachine movementStateMachine;
    public CharacterController characterController;
    public PlayerControls playerControls;
    public Transform target;
    public Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded
    public Vector3 knockBackDirection;
    public Vector3 impact = Vector3.zero;
    public bool isDebugging = false;
    public bool isRunning = false; //TODO: REMOVE THIS VARIABLE, THIS KIND OF INFORMATION SHOULD BE KEPT IN THE STATE MACHINE.
    public bool isBeingControlled = false; // Indicates weather user inputs are currently controlling this character.
    public float currentSpeed = 50f;
    public float walkSpeed = 20;
    public float runSpeed = 100;
    public int jumpCount = 0;
    public int maxJumps = 2;
    public float jumpHeight = 20;
    public float gravity = -8f;
    public float turnSmoothTime = 0.1f;
    public float rotationSpeed = 100;
    public float distanceFromTarget;
    public float minDistanceFromTarget = 20;
    public int knockBackForce = 0;
    public int enterChaseDistance = 100;
    public int exitChaseDistance = 160;
    public int attackRange = 10;
    public float jumpStateTime = 1f;
    public float sprintTimer = 0f;
    public float sprintLimit = 3f;
    public float mass = 3.0f;
    public float stunTimer = 0f;
    public float stunDuration = 1f;


    //Using constructor here because when accessing stateMachine in the derrived classes in Start() Method.
    //Even tho awake is usually called first, inheritance is not considered.
    public MovementComponent()
    {
        movementStateMachine = new StateMachine();
    }

    protected virtual void Awake()
    {
        if (playerControls == null) playerControls = new PlayerControls();

        characterController = GetComponent<CharacterController>();
        updateMoveSpeed();
        ResetSprintTimer();
    }

    protected virtual void Update()
    {
        movementStateMachine.CurrentState.HandleInput();
        movementStateMachine.CurrentState.LogicUpdate();
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

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    public float getDistanceFromTarget()
    {
        if (this.target == null) { return 0f; }
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

    public PlayerEntity getPlayerScript()
    {
        if (playerScript == null)
        {
            playerScript = PlayerManager.Instance.getPlayerScript();
        }
        return playerScript;
    }

    public void Knockback()
    {
        Vector3 kb_direction = this.knockBackDirection;
        int kb_force = this.knockBackForce;
        this.impact = Vector3.zero;

        if (!(knockBackDirection.magnitude > 0.1))
        {
            kb_direction = this.transform.forward * -1;
        }

        //Add impact
        kb_direction.Normalize();
        if (kb_direction.y < 0) kb_direction.y = -kb_direction.y; // reflect down force on the ground
        this.impact += kb_direction.normalized * kb_force / mass;


        //AddJumpVelocity(); //TODO: Make this work with the knockback variable passed from Recieve damage to have a variable knockback amount.
    }

    public void ConsumeImpact()
    {
        //Debug.Log("consuming impact.");
        if (impact.magnitude > 0.2) characterController.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        this.impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public virtual void toggleRun()
    {
        if (movementStateMachine.CurrentState.ToString() != "FallingState" && movementStateMachine.CurrentState.ToString() != "JumpingState")
        {
            if (isRunning ? isRunning = false : isRunning = true) ;
            updateMoveSpeed();
        }
    }

    public void updateMoveSpeed()
    {
        //TODO: This shoudl use the state
        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    /// <summary>
    /// Regenrate Sprint Meter, for actions like resting or sitting idle.
    /// </summary>
    public void RegenerateStaminaMeter()
    {
        if (!isBeingControlled)
            return;

        sprintTimer += 1 * Time.deltaTime;

        //Do not let it go above the limit
        if (sprintTimer > sprintLimit)
            sprintTimer = sprintLimit;
    }


    /// <summary>
    /// Consume Sprint Meter, decrementing the value.
    /// If stamina value reaches hits 0 or lower, switch to walking.
    /// </summary>
    public void ConsumeSprintMeter()
    {
        if (!isBeingControlled)
            return;

        sprintTimer -= 1 * Time.deltaTime;

        if (sprintTimer <= 0)
        {
            sprintTimer = 0;
            toggleRun();
            if (isDebugging)
                Debug.Log("Sprint Ended");
        }

        //Debug.Log("Sprint Timer = " + sprintTimer);
    }



    protected void ResetSprintTimer()
    {
        sprintTimer = sprintLimit;
    }

    public virtual bool IsGrounded()
    {
        return characterController.isGrounded;
    }
    private void OnEnable()
    {
        //OnEnable gets called before Awake so this is needed.
        if (playerControls == null) playerControls = new PlayerControls();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        //playerControls.Disable();
    }
}
