using Polyperfect.Common;
using System.Collections;
using UnityEngine;

//Player movement component contains a state machine, a working group states relevent to the player, varaible, and functions relevent to player movement.
//It is the "Brain" of player movement.
public class PlayerMovementComponent : MovementComponent
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
    public bool isOnSteepSlope = false;
    public float sphereRad = 5;
    public float sphereDist = 5;
    public Vector3 movementDirection;
    private RaycastHit slopeHit;
    private RaycastHit steepSlopeHit;

    //Dashing Variables
    public float dashSpeed = 40f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 0.5f;
    private float lastDashTime = -Mathf.Infinity;

    //For matching surface rotation on terrain when moving.
    //[SerializeField] private bool matchSurfaceRotation = true;
    //[SerializeField] private int surfaceRotationSpeed = 20;

    protected override void Awake()
    {
        base.Awake();

        //Match surface rotation to the terrain. 
        //if (matchSurfaceRotation && transform.childCount > 0)
        //{
        //    transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
        //}
    }

    private void Start()
    {
        this.stunDuration = 1f;
        activeMount = null;
        isBeingControlled = true;

        //Initialize the players states.
        standing = new IdleState(movementStateMachine, this);
        jumping = new JumpingState(movementStateMachine, this);
        riding = new RidingState(movementStateMachine, this);
        falling = new FallingState(movementStateMachine, this);
        moving = new MovingState(movementStateMachine, this);
        emote = new EmoteState(movementStateMachine, this);
        stun = new StunnedState(movementStateMachine, this);

        if (characterController.isGrounded)
            movementStateMachine.Initialize(standing);
        else
            movementStateMachine.Initialize(jumping);
    }

    protected override void Update()
    {
        playerScript.UpdateStaminaUI();

        bool dashKeyCaptured = playerControls.Player.LeftShift.WasPressedThisFrame();  // Assumes you have a Dash action set up in your PlayerControls
        if (dashKeyCaptured && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        base.Update();
    }

    /// <summary>
    /// Performs a dash in the direction of the player's movement input based on the camera's orientation.
    /// The dash lasts for a specified duration and moves the player at a defined dash speed.
    /// </summary>
    /// <returns>IEnumerator for the coroutine behavior of the dash.</returns>
    private IEnumerator Dash()
    {
        float dashStartTime = Time.time;

        // Get player input direction for dashing
        float horizontal = playerControls.Player.Movement.ReadValue<Vector2>().x;
        float vertical = playerControls.Player.Movement.ReadValue<Vector2>().y;

        // Calculate the dash direction based on the camera's orientation
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * vertical + right * horizontal;

        while (Time.time < dashStartTime + dashDuration)
        {
            Vector3 dashVelocity = desiredMoveDirection * dashSpeed;
            characterController.Move(dashVelocity * Time.deltaTime);
            yield return null;
        }

        lastDashTime = Time.time;
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

    public void MoveToMountedPosition()
    {
        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + this.activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }

    /// <summary>
    /// Handles player movement based on user input.
    /// </summary>
    public void MovePlayerViaInput()
    {
        Vector3 inputDirection = GetInputDirection();

        if (inputDirection.sqrMagnitude >= 0.01f)
        {
            RotatePlayer(inputDirection);
            movementDirection = CalculateMoveDirection(inputDirection);

            if (isOnSteepSlope)
                SlideDownSlope();
            else
                MoveCharacter();
        }
        // Combined check for no movement input and being on a steep slope.
        else if (isOnSteepSlope)
        {
            SlideDownSlope();
        }
    }

    /// <summary>
    /// Retrieves and normalizes the movement input.
    /// </summary>
    /// <returns>Normalized movement direction.</returns>
    private Vector3 GetInputDirection()
    {
        float horizontal = playerControls.Player.Movement.ReadValue<Vector2>().x;
        float vertical = playerControls.Player.Movement.ReadValue<Vector2>().y;
        return new Vector3(horizontal, 0f, vertical).normalized;
    }

    /// <summary>
    /// Rotates the player to face the desired movement direction.
    /// </summary>
    /// <param name="direction">Desired movement direction.</param>
    private void RotatePlayer(Vector3 direction)
    {
        // Calculate desired rotation angle considering camera's Y-axis rotation.
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        // Smoothly transition to the desired angle using damping.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);

        // Apply rotation.
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    /// <summary>
    /// Calculates the movement direction after incorporating any rotations.
    /// </summary>
    /// <param name="direction">Initial movement direction.</param>
    /// <returns>Calculated movement direction.</returns>
    private Vector3 CalculateMoveDirection(Vector3 direction)
    {
        //Calculates the target rotation angle for the player based on the movement direction and the camera's orientation
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        //Creating a forward direction vector that has been rotated by the targetAngle on the Y-axis.
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    /// <summary>
    /// Handles the logic to slide the player down steep slopes.
    /// </summary>
    private void SlideDownSlope()
    {
        Vector3 slopeDir = Vector3.up - steepSlopeHit.normal * Vector3.Dot(Vector3.up, steepSlopeHit.normal);
        movementDirection = slopeDir * (-slopeSpeed * Time.deltaTime);
        movementDirection.y = movementDirection.y - steepSlopeHit.point.y;
        MoveCharacter();
    }

    /// <summary>
    /// Applies the movement to the player character.
    /// </summary>
    private void MoveCharacter()
    {
        characterController.Move(movementDirection.normalized * (currentSpeed) * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        var currentPos = transform.position + (transform.forward * groundCheckForwardOffset) + (transform.up * groundCheckVerticleOffset);
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(currentPos, sphereRad);
    }

    public override bool IsGrounded()
    {
        isOnSteepSlope = false;
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
                isOnSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(backOffsetPosition, backDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                isOnSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(leftOffsetPosition, leftDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                isOnSteepSlope = false;
                return true;
            }
        }
        if (Physics.Raycast(rightOffsetPosition, rightDown, out slopeHit, groundCheckDistance))
        {
            if (slopeHit.transform.gameObject.layer == LayerMask.NameToLayer("Collectable")) return false;
            if (!checkSteepSlope(slopeHit))
            {
                isOnSteepSlope = false;
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
            isOnSteepSlope = true;
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
        obj.transform.rotation = Quaternion.Euler(0, euler.y, 0);
    }

    //public void Knockback(float knockBackForce)
    //{
    //    //Add Impact
    //    Vector3 direction = this.transform.forward * -1; //Need to make this direction
    //    Vector3 up = this.transform.up;
    //    up.Normalize();
    //    direction.Normalize();
    //    direction.y = up.y;
    //    var impact = Vector3.zero;
    //    impact += direction.normalized * knockBackForce;

    //    //Apply vector to object
    //    playerMovementComponent.characterController.Move(impact * Time.deltaTime);
    //}
}
