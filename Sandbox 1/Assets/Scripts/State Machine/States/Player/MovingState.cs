using UnityEngine;

public class MovingState : GroundedState
{
    PlayerAnimator animator;
    private bool jump;

    public MovingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().playerAnimator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        if (movementComponent.isDebugging)
        {
            Debug.Log("GROUNDED MOVING");
        }
    }

    public override void HandleInput()
    {

        base.HandleInput();
        if (movementComponent.isRunning)
        {
            movementComponent.ConsumeSprintMeter();
            animator.setRunningAnimation();
        }
        else
        {
            movementComponent.RegenerateStaminaMeter();
            animator.setWalkingAnimation();
        }

        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();

    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (movementComponent.stunTimer != 0)
        {
            stateMachine.ChangeState(movementComponent.stun);
        }
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
        else if(!movementComponent.isMoving()){
            this.stateMachine.ChangeState(movementComponent.standing);
        }
        else if (!movementComponent.IsGrounded())
        {
             this.stateMachine.ChangeState(movementComponent.falling);   
        }
    }
}
