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
            movementComponent.wanderscript.resetOrigin();
            movementComponent.wanderscript.UpdateAI();
            movementComponent.wanderscript.enabled = true;
        }
        movementComponent.naveMeshAgent.enabled = true;
        movementComponent.isBeingControlled = false;
        movementComponent.isMoving = false;
        Debug.Log("Entered ENEMY Standing state");
        base.Enter();
    }

    public override void Exit()
    {
    }

    public override void HandleInput() //Do stuff
    {
                movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
    }

    //Get next state
    public override void LogicUpdate()
    {
        float distance = Vector3.Distance(movementComponent.getPlayerScript().transform.position , movementComponent.characterController.transform.position);

        if (distance < 100)
        {
            stateMachine.ChangeState(movementComponent.chasing);
        }

        base.LogicUpdate();
    }
}
