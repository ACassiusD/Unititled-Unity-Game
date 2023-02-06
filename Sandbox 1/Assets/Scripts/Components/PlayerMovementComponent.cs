using UnityEngine;
using Polyperfect.Common;

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
    public StunnedState stun;
    public Camera cam;
    //Only movement variables specific to player should go here
    public bool isRiding = false;
    public Mount activeMount; //This might need to be moved out
    public float groundCheckDistance = 0.6f;
    public float groundCheckOutwardOffset = 0.22f;
    public float groundCheckVerticleOffset = 0f;
    public float groundCheckForwardOffset = 0;
    public float groundCheckTime = 0.1f;
    public float groundCheckTimer = 0f; //How long after jumping can we check for isGrounded again.
    public float slopeLimit = 45;
    public float slopeSpeed = 50;
    public bool onSteepSlope = false;
    public float sphereRad = 5;
    public float sphereDist = 5;
    public Vector3 moveDir;
    private float groundRayDistance = 1;
    private RaycastHit slopeHit;
    private RaycastHit steepSlopeHit;

    protected void Awake()
    {
        base.Awake();
    }

    public bool isMoving()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = playerControls.Player.Movement.ReadValue<Vector2>().x;
        float vertical = playerControls.Player.Movement.ReadValue<Vector2>().y;

        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        return (direction.magnitude >= 0.1f);
    }

    protected void Update()
    {
        base.Update();
    }

    private void Start()
    {
        this.stunDuration = 1f;
        activeMount = null;
        isBeingControlled = true;
        //Initialize the players states.
        standing = new IdleState(stateMachine, this);
        jumping = new JumpingState(stateMachine, this);
        riding = new RidingState(stateMachine, this);
        falling = new FallingState(stateMachine, this);
        moving = new MovingState(stateMachine, this);
        emote = new EmoteState(stateMachine, this);
        stun = new StunnedState(stateMachine, this);

        if (characterController.isGrounded)
            stateMachine.Initialize(standing); 
        else
            stateMachine.Initialize(jumping);
    }

    //VIOLATES SINGLE USE PRINCIPAL, MOVE INTO SEPERATE CLASS FOR STAMINA MAINTENANCE. 
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


    //VIOLATES SINGLE USE PRINCIPAL, MOVE INTO SEPERATE CLASS FOR STAMINA MAINTENANCE. 
    public void SetActiveMount(Mount mount)    //This should not be in the move controller.
    {
        this.activeMount = mount;
        isRiding = true;
    }

    public void MovePlayerViaInput()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = playerControls.Player.Movement.ReadValue<Vector2>().x;
        float vertical = playerControls.Player.Movement.ReadValue<Vector2>().y;

       // Debug.Log(horizontal + vertical);

        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        if (direction.magnitude >= 0.1f)
        {
            //Mathf.Atan2(direction.x, direction.z) - Gives us the angle in radians our player needs to turn
            //Mathf.Rad2Deg Update the angle to degrees
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            //Smooth the angle to not immediatly point towards it.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);
            //Rotate our player on the Y axis by the angle 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //reference for more information - https://www.youtube.com/watch?v=4HpC--2iowE
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Slide down slopes
            if (onSteepSlope)
            {
                Vector3 slopeDir = Vector3.up - steepSlopeHit.normal * Vector3.Dot(Vector3.up, steepSlopeHit.normal); 
                moveDir = slopeDir * (-slopeSpeed * Time.deltaTime);
                moveDir.y = moveDir.y - steepSlopeHit.point.y;
                characterController.Move(moveDir.normalized * (moveSpeed) * Time.deltaTime);
            }
            else
            {
                characterController.Move(moveDir.normalized * (moveSpeed) * Time.deltaTime);
            }
        }
        else
        {
            //TODO: MAKE SLOPE SLIDE ACCELLERATE
            //FIX SPEED ISSUES.
            if (onSteepSlope)
            {
                Vector3 slopeDir = Vector3.up - steepSlopeHit.normal * Vector3.Dot(Vector3.up, steepSlopeHit.normal);
                var moveDir = slopeDir * (-slopeSpeed * Time.deltaTime);
                moveDir.y = moveDir.y - steepSlopeHit.point.y;
                characterController.Move(moveDir.normalized * (moveSpeed) * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var currentPos = transform.position + (transform.forward * groundCheckForwardOffset) + (transform.up * groundCheckVerticleOffset);
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(currentPos, sphereRad);
    }

    public override bool IsGrounded()
    {
        onSteepSlope = false;
        var currentPos = transform.position;
        var frontOffsetPosition = transform.position + (transform.forward * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var backOffsetPosition = transform.position + (-transform.forward * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var leftOffsetPosition = transform.position + (-transform.right * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);
        var rightOffsetPosition = transform.position + (transform.right * groundCheckOutwardOffset) + (transform.up * groundCheckVerticleOffset);

        var forwardDown = this.transform.forward - this.transform.up;
        var backDown = (-this.transform.forward) - this.transform.up;
        var rightDown = this.transform.right - this.transform.up; ;
        var leftDown = (-this.transform.right) - this.transform.up; ;

        if (isDebugging)
        {
            Debug.DrawRay(frontOffsetPosition, forwardDown * groundCheckDistance, Color.red);
            Debug.DrawRay(backOffsetPosition, backDown * groundCheckDistance, Color.blue);
            Debug.DrawRay(leftOffsetPosition, leftDown * groundCheckDistance, Color.yellow);
            Debug.DrawRay(rightOffsetPosition, rightDown * groundCheckDistance, Color.magenta);
            Debug.DrawRay(currentPos, (-Vector3.up), Color.green);
        }

        //if (Physics.Raycast(currentPos, -Vector3.up, out slopeHit, groundCheckDistance))
        //{
        //    if (!checkSteepSlope(slopeHit))
        //    {
        //        onSteepSlope = false;
        //        return true;
        //    }
        //}

        if (Physics.Raycast(frontOffsetPosition, forwardDown, out slopeHit, groundCheckDistance))
        {
            //Do not check for ground collision on collectables.
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
           
            if (!checkSteepSlope(slopeHit))
            {
                onSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(backOffsetPosition, backDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                onSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(leftOffsetPosition, leftDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                onSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(rightOffsetPosition, rightDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                onSteepSlope = false;
                return true;
            }
        }
        return false;
    }

    protected bool checkSteepSlope(RaycastHit hit)
    {
        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
        if (slopeAngle > slopeLimit)
        {
            steepSlopeHit = hit;
            onSteepSlope = true;
            return true;
        }
        return false;
    }

    public void EnabledSurfaceRotation()
    {
        GetComponentInChildren<Common_SurfaceRotation>().enabled = true;
    }

    public void DisableSurfaceRotation()
    {
        var surfaceRoationObj = GetComponentInChildren<Common_SurfaceRotation>();
        var obj = transform.GetChild(0).gameObject;
        surfaceRoationObj.enabled = false;
        var euler = obj.transform.rotation.eulerAngles;
        obj.transform.rotation = Quaternion.Euler(0, euler.y ,0);
    }
}
