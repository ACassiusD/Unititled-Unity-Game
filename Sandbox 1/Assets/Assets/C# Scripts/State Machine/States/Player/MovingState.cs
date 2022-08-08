using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : GroundedState
{
    PlayerAnimator animator;
    private bool jump;

    public MovingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        if (movementComponent.isDebugging)
        {
            Debug.Log("GROUNDED MOVING");
        }
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {

        base.HandleInput();
        if (movementComponent.isRunning)
        { 
            animator.setRunningAnimation();
        }
        else
        {           
            animator.setWalkingAnimation();
            movementComponent.RegenerateStamina();
        }

        jump = Input.GetButtonDown("Jump");

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
        else if(!movementComponent.isMoving){
            this.stateMachine.ChangeState(movementComponent.standing);
        }
        else
        {
            if (!movementComponent.IsGrounded())
            {
                this.stateMachine.ChangeState(movementComponent.falling);
            }
        }
    }
}
