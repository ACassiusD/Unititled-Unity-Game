using UnityEngine;

public class EnemyChasingState : EnemyState
{
    public EnemyChasingState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        movementComponent.enabled = true;
        movementComponent.naveMeshAgent.enabled = false;
        if (movementComponent.wanderscript != null)
        {
            movementComponent.wanderscript.enabled = false;
        }
        movementComponent.isMoving = true;
        movementComponent.setTarget(EnemyMovementComponent.playerScript.transform);
        Debug.Log("Entered Enemy Chasing state");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    //Do state stuff
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
        if (movementComponent.IsInRangeOfPlayer() == false)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        base.LogicUpdate();
    }
}
