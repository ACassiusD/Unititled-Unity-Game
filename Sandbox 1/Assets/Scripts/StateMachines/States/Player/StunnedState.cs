using UnityEngine;

public class StunnedState : PlayerState
{
    PlayerAnimator animator;

    public StunnedState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
        animator = movementComponent.getPlayerScript().playerAnimator;
    }

    public override void Enter()
    {
        Debug.Log("Entered Stun state");
        animator.setStunnedAnimation();
        movementComponent.stunTimer = movementComponent.stunDuration;
        base.Enter();
    }

    public override void HandleInput()
    {
        movementComponent.stunTimer -= Time.deltaTime;
        //if (movementComponent.isDebugging)
        Debug.Log("Time = " + movementComponent.stunTimer);
    }

    public override void LogicUpdate()
    {
        if (movementComponent.stunTimer <= 0)
        {
            Debug.Log("Stun timer ended");
            movementComponent.stunTimer = 0;
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.LogicUpdate();
    }

    public override void Exit()
    {
        //if (movementComponent.isDebugging)
        Debug.Log("Enemy Exit Knockback state");
    }


}

