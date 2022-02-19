using UnityEngine;

public class JumpingState : State
{
    private bool grounded;
    private int jumpParam = Animator.StringToHash("Jump");
    private int landParam = Animator.StringToHash("Land");
    Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded.
    CharacterController controller;
    bool justEntered = false;
    [SerializeField]
    public bool isTest = true;

    private bool jump;
    bool isGrounded = false;

    public JumpingState(StateMachine stateMachine, MovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        isGrounded = false;
        justEntered = true;
        this.movementComponent.jumpCount++;
        controller = movementComponent.GetComponent<CharacterController>();
        Debug.Log("Entered jumping state");
        base.Enter();
        //Initial Jump
        velocity.y += Mathf.Sqrt(movementComponent.jumpHeight * -2f * movementComponent.gravity);
    }
    public override void HandleInput()
    {
        Jump();
        base.HandleInput();
        jump = Input.GetButtonDown("Jump");
    }


    public override void LogicUpdate()
    {
        if (isGrounded)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.HandleInput();
    }

    private void Jump()
    {
        if (!justEntered)
        {
            isGrounded = controller.isGrounded;
        }

        //Grounded reset velocity check
        if (isGrounded && velocity.y < 0 && !justEntered)
        {
            velocity.y = -2f;
        }

        //Gravity Check
        if (!isGrounded && !justEntered)
        {
            velocity.y += movementComponent.gravity * Time.deltaTime;
        }

        //Check for double jump
        if (!isGrounded && jump && !justEntered)
        {
            movementComponent.jumpCount++;
            velocity.y = Mathf.Sqrt(movementComponent.jumpHeight * -2f * movementComponent.gravity);
        }

        controller.Move(velocity * Time.deltaTime);

        movementComponent.Move();

        justEntered = false;
    }
}

