using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockbackState : EnemyState
{
    public EnemyKnockbackState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy entered knockback state");
        movementComponent.Knockback();
        movementComponent.stunTimer = movementComponent.stunDuration;
        base.Enter();
    }

    public override void HandleInput()
    {
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.stunTimer -= Time.deltaTime;
        Debug.Log("Time = " + movementComponent.stunTimer);
    }

    public override void LogicUpdate()
    {
        if (movementComponent.characterController.isGrounded && movementComponent.stunTimer <= 0)
        {
            Debug.Log("Stun timer ended");
            movementComponent.stunTimer = 0;

            float distance = Vector3.Distance(movementComponent.getPlayerScript().transform.position, movementComponent.characterController.transform.position);

            if (distance < movementComponent.exitChaseDistance)
            {
                Debug.Log("Enemy moved to chasing state.");
                stateMachine.ChangeState(movementComponent.chasing);
            }
            else
            {
                Debug.Log("Enemy moved to standing state.");
                stateMachine.ChangeState(movementComponent.standing);
            }
        }
        base.LogicUpdate();
    }

    public override void Exit()
    {
        movementComponent.inHitStun = false;
    }
}
