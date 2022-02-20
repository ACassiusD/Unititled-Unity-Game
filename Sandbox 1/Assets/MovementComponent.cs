using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public StateMachine movementSM;
    public StandingState standing;
    public DuckingState ducking;
    public JumpingState jumping;
    public int jumpCount = 0;
    public float moveSpeed = 50f;
    public float rotationSpeed;
    CharacterController characterController;
    public float turnSmoothTime = 0.1f;
    public Transform cam;
    public int maxJumps = 2;
    public float jumpHeight = 20;
    public float gravity = -8f;

    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        movementSM = new StateMachine();
        standing = new StandingState(movementSM, this);
        ducking = new DuckingState(movementSM, this);
        jumping = new JumpingState(movementSM, this);

        if (characterController.isGrounded)
        {
            movementSM.Initialize(standing);
        }
        else
        {
            movementSM.Initialize(jumping);
        }
    }

    private void Update()
    {
        movementSM.CurrentState.HandleInput();
        movementSM.CurrentState.LogicUpdate();
    }

    public void Move()
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

    public void ResetMoveParams()
    {
    }

}
