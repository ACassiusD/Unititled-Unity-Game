using UnityEngine;

public class MountJumpingState : MountState
{
    private int jumpParam = Animator.StringToHash("Jump");
    private int landParam = Animator.StringToHash("Land");
    CharacterController controller;
    bool justEntered = false;
    private bool jump;

    // Start is called before the first frame update
    public MountJumpingState(StateMachine stateMachine, MountMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    //ENTER LOGIC
    public override void Enter()
    {
        //Initial Jump
        movementComponent.isMoving = false;
        movementComponent.AddJumpVelocity();
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
                movementComponent.MountJump();
            }
        }
        movementComponent.AddVelocityAndMove();
        movementComponent.MoveMountViaInput();
        justEntered = false;
        base.HandleInput();
    }


    public override void LogicUpdate()
    {
        if (movementComponent.characterController.isGrounded)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.LogicUpdate();
    }

}
