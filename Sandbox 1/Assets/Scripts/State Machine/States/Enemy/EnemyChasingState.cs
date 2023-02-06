using UnityEngine;

public class EnemyChasingState : EnemyState
{
    public EnemyChasingState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        if (movementComponent.isDebugging)
        {
            //Debug.Log("Enemy entered Chasing state");
        }

        movementComponent.naveMeshAgent.enabled = false;
        if (movementComponent.wanderscript != null)
        {
            movementComponent.wanderscript.enabled = false;
        }
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
            //Debug.Log("Enemy going to knockback state.");
            stateMachine.ChangeState(movementComponent.knockback);
        }

        if (movementComponent.InChaseRange() == false)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }

        if(movementComponent.InAttackRange() == true)
        {
            //Needs to be moved to a attacking state. Is attacking state going to be a new state machine?? or keep using movement state machine for enemies?
            if (movementComponent.isDebugging)
            {
                Debug.Log("Enemy Began Attacking.");
            }
            movementComponent.getEnemyScript().testFunction();
        }
        base.LogicUpdate();
    }
}
