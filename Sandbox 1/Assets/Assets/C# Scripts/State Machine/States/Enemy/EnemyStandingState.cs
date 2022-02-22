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
        Debug.Log("Entered ENEMY Standing state");
        base.Enter();
    }

    public override void Exit()
    {
    }

    public override void HandleInput() //Do stuff
    {
        //stand
    }

    //Get next state
    public override void LogicUpdate()
    {
        float distance = Vector3.Distance(movementComponent.playerTransform.position, movementComponent.characterController.transform.position);

        if (distance < 100)
        {
            stateMachine.ChangeState(movementComponent.chasing);
        }

        base.LogicUpdate();
    }
}
