using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandingState : EnemyState
{
    public EnemyStandingState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        if (movementComponent.wanderscript != null)
        {
            movementComponent.wanderscript.enabled = true;
            movementComponent.getEnemyScript().enemyAnimator.setIdleAnimation();
            movementComponent.getEnemyScript().enemyAnimator.ClearAnimation();
            movementComponent.wanderscript.resetOrigin();
            movementComponent.wanderscript.UpdateAI();
        }
        movementComponent.naveMeshAgent.enabled = true;
        movementComponent.isBeingControlled = false;
        movementComponent.isMoving = false;
        Debug.Log("Entered ENEMY Standing state");
        base.Enter();
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
        float distance = movementComponent.getDistanceFromTarget();

        if (movementComponent.inHitStun && movementComponent.knockBackForce > 0)
        {
            stateMachine.ChangeState(movementComponent.knockback);
        }

        if (distance < movementComponent.enterChaseDistance)
        {
            stateMachine.ChangeState(movementComponent.chasing);
        }

        if(movementComponent.getEnemyScript().attacked && distance <= movementComponent.exitChaseDistance)
        {
            stateMachine.ChangeState(movementComponent.chasing);
            movementComponent.getEnemyScript().resetAttackedState();
        }

        base.LogicUpdate();
    }

    public override void Exit()
    {
    }
}
