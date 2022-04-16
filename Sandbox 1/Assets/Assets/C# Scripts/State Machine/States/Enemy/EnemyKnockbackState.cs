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
        movementComponent.Knockback();
        movementComponent.stunTimer = 0.5f;
        //start stun duration

        Debug.Log("Enemy knocked back");
        Debug.Log("Stun timer started");
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

            if (distance < 100)
            {
                stateMachine.ChangeState(movementComponent.chasing);
            }
            else
            {
                stateMachine.ChangeState(movementComponent.standing);
            }
        }
        base.LogicUpdate();
    }

    public override void Exit()
    {
    }
}
