using UnityEngine;

public class FallingState : PlayerState
{
    private bool jump;
    PlayerAnimator animator;
    private bool justEntered = true;

    public FallingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        if (movementComponent.isDebugging)
        {
            Debug.Log("FALLING");
        }
        movementComponent.getPlayerScript().animator.setFallingAnimation();
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {
        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();
        //if (!justEntered)
        //{
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        if (jump)
        {
            movementComponent.MidAirJump();
        }
        //}
        movementComponent.AddVelocityAndMove();
        movementComponent.MovePlayerViaInput();
        base.HandleInput();
    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        if (movementComponent.IsGrounded() && movementComponent.isMoving)
        {
            stateMachine.ChangeState(movementComponent.moving);
        }
        else if(movementComponent.IsGrounded())
        {
              stateMachine.ChangeState(movementComponent.standing);
        }
        if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
        base.LogicUpdate();
    }

}
