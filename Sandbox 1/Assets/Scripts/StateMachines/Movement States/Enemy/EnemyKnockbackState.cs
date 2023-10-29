using UnityEngine;

public class EnemyKnockbackState : EnemyState
{
    public EnemyKnockbackState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        if (movementComponent.isDebugging)
            Debug.Log("Enemy entered Knockback state");

        movementComponent.Knockback();
        movementComponent.stunTimer = movementComponent.stunDuration;
        base.Enter();
    }

    public override void HandleInput()
    {
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.ConsumeImpact();
        movementComponent.stunTimer -= Time.deltaTime;
        if (movementComponent.isDebugging)
        {
            if (movementComponent.isDebugging)
                Debug.Log("Time = " + movementComponent.stunTimer);
        }
    }

    public override void LogicUpdate()
    {
        if (movementComponent.characterController.isGrounded && movementComponent.stunTimer <= 0)
        {
            movementComponent.stunTimer = 0;

            float distance = Vector3.Distance(movementComponent.getPlayerScript().transform.position, movementComponent.characterController.transform.position);

            if (distance < movementComponent.exitChaseDistance)
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
        if (movementComponent.isDebugging)
            Debug.Log("Enemy Exit Knockback state");
    }


}
