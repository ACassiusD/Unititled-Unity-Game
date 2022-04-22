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
        if (movementComponent.isDebugging)
        {
            Debug.Log("Entered jumping state");
        }

        movementComponent.AddJumpVelocity();
        movementComponent.getPlayerScript().animator.setJumpingAnimation();
        justEntered = true;
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
        movementComponent.MovePlayerViaInput();
        base.HandleInput();
    }


    public override void LogicUpdate()
    {
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        } 
        if (movementComponent.isGrounded() && !justEntered)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        if (justEntered)
        {
            justEntered = false;
        }
        base.LogicUpdate();
    }


}

