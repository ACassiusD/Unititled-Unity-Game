using UnityEngine;

public class IdleState : GroundedState
{
    private bool jump;
    PlayerAnimator animator;
    private bool justEntered = true;

    public IdleState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        justEntered = true;
        if (movementComponent.isDebugging)
        {
            Debug.Log("IDLE");
        }
        animator.setIdleAnimation();
        base.Enter();
        jump = false;
        if (movementComponent.isRunning)
        {
            //Turn to walking state so stamina can recharge.
            movementComponent.toggleRun(true);
        }
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {
        movementComponent.IsGrounded();
        movementComponent.RegenerateStamina();
        base.HandleInput();
        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();
    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        base.LogicUpdate();
  
        if(movementComponent.stunTimer != 0)
        {
            stateMachine.ChangeState(movementComponent.stun);
        }
        if (movementComponent.isMoving){
            stateMachine.ChangeState(movementComponent.moving);
        }
        if (movementComponent.isRiding){
            stateMachine.ChangeState(movementComponent.riding);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps){
            stateMachine.ChangeState(movementComponent.jumping);
        }
        else if (movementComponent.playerControls.Player.Y.WasPerformedThisFrame()) { 
            stateMachine.ChangeState(movementComponent.emote);
        }
    }
}
