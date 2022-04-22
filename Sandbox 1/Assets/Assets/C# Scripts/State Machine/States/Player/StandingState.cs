using UnityEngine;

public class StandingState : GroundedState
{
    private bool jump;
    PlayerAnimator animator;
    private bool justEntered = true;

    public StandingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        justEntered = true;
        if (movementComponent.isDebugging)
        {
            Debug.Log("entered standing state.");
        }
        animator.setIdleAnimation();
        base.Enter();
        jump = false;
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {
        movementComponent.isGrounded();
        base.HandleInput();
        jump = Input.GetButtonDown("Jump");
        animator.setIdleAnimation(); //default for stadning state
        if (justEntered)
        {
            animator.setIdleAnimation();
            justEntered = false;
        }
        else
        {
            if (movementComponent.isMoving)
            {
                if (movementComponent.isRunning)
                    animator.setWalkingAnimation();
                else
                    animator.setWalkingAnimation();
            }
        }
    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
    }

}
