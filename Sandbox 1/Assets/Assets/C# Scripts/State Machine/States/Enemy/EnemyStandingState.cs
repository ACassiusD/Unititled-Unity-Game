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
        float distance = Vector3.Distance(movementComponent.getPlayerScript().transform.position , movementComponent.characterController.transform.position);

        if (movementComponent.inHitStun && movementComponent.knockBackForce > 0)
        {
            stateMachine.ChangeState(movementComponent.knockback);
        }

        if (distance < 100)
        {
            stateMachine.ChangeState(movementComponent.chasing);
        }

        base.LogicUpdate();
    }

    public override void Exit()
    {
    }
}
