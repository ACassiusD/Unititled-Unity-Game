using UnityEngine;

public class JumpingState : PlayerState
{
    private int jumpParam = Animator.StringToHash("Jump");
    private int landParam = Animator.StringToHash("Land");
    CharacterController controller;
    bool justEntered = false;
    [SerializeField]
    public bool isTest = true;
    public bool isRiding = true;

    private bool jump;


    public JumpingState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {   
        //Initial Jump
        movementComponent.AddJumpVelocity();
        justEntered = true;
        Debug.Log("Entered jumping state");
        base.Enter();
    }

    public override void Exit()
    {
        movementComponent.jumpCount = 0;
    }

    public override void HandleInput()
    {
        jump = Input.GetButtonDown("Jump");
        if (!justEntered)
        {
            movementComponent.ZeroYVelocityIfGrounded();
            movementComponent.addGravity();
            if (jump)
            {
                movementComponent.MidAirJump();
            }
        }
        movementComponent.AddVelocityAndMove();
        movementComponent.MoveByInput();
        justEntered = false;
        base.HandleInput();
    }


    public override void LogicUpdate()
    {
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        }
        if (movementComponent.characterController.isGrounded)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.LogicUpdate();
    }


}

