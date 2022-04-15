using UnityEngine;

public class EnemyChasingState : EnemyState
{
    public EnemyChasingState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        // Debug.Log("Entered Enemy Chasing state");
        movementComponent.naveMeshAgent.enabled = false;
        if (movementComponent.wanderscript != null)
        {
            movementComponent.wanderscript.enabled = false;
        }
        movementComponent.isMoving = true;
        movementComponent.setTarget(EnemyMovementComponent.playerScript.transform);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.MoveTowardsTarget();
    }

    //Get next state
    public override void LogicUpdate()
    {
        if (movementComponent.inHitStun && movementComponent.knockBackForce > 0)
        {
            stateMachine.ChangeState(movementComponent.knockback);
        }

        if (movementComponent.IsInRangeOfPlayer() == false)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.LogicUpdate();
    }
}
