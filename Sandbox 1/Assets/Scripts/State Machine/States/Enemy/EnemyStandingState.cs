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
        if (movementComponent.isDebugging)
        {
            Debug.Log("Enemy entered Standing state");
        }

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

        base.Enter();
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
        float distance = movementComponent.getDistanceFromTarget();

        if (distance < movementComponent.enterChaseDistance)
        {
            stateMachine.ChangeState(movementComponent.chasing);
        }

        if (movementComponent.getEnemyScript().attacked && distance <= movementComponent.exitChaseDistance)
        {
            stateMachine.ChangeState(movementComponent.chasing);
            movementComponent.getEnemyScript().resetAttackState();
        }

        base.LogicUpdate();
    }

    public override void Exit()
    {
    }
}
