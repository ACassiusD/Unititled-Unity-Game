using UnityEngine;
using Polyperfect.Animals;
using UnityEngine.AI;

public class MountMoveComponent : MoveComponent
{
    public MountStandingState standing;
    public MountWanderingState wandering;
    public MountJumpingState jumping;
    Vector3 velocity = new Vector3(); //gravity force
    public Transform cam;
    public Animal_WanderScript wanderscript;
    public float mountWalkSpeed = 20;
    public float mountRunSpeed = 100;
    public NavMeshAgent naveMeshAgent;
    public Mount mountScript;


    private void Start()
    {
        isEnabled = true;
        isBeingControlled = false;
        wanderscript = this.GetComponent<Animal_WanderScript>();
        naveMeshAgent = this.GetComponent<NavMeshAgent>();
        wandering = new MountWanderingState(stateMachine, this);
        standing  = new MountStandingState(stateMachine, this); 
        jumping = new MountJumpingState(stateMachine, this);
        
        updateMoveSpeed();
        
        if (isBeingControlled)
        {
            stateMachine.Initialize(standing);
        }
        else
        {
            stateMachine.Initialize(wandering);
        }
    }

    public void ResetMount()
    {
        this.Start();
    }

    protected void Update()
    {
        if (isEnabled)
        {
            toggleRun();
            base.Update();
        }
    }

    public void MountJump() {
        MidAirJump();
    }

    public void MoveMountViaInput()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

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
            
        }
        else
        {
            isMoving = false;
        }
    }

    public void toggleRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isRunning ? isRunning = false : isRunning = true);
            updateMoveSpeed();
        }
    }

    public void updateMoveSpeed()
    {
        if (isRunning)
        {
            moveSpeed = mountRunSpeed;
        }
        else
        {
            moveSpeed = mountWalkSpeed;
        }
    }

    public Mount getMountScript()
    {
        if (mountScript == null)
        {
            mountScript = this.GetComponent<Mount>();
        }
        return mountScript;
    }
}
