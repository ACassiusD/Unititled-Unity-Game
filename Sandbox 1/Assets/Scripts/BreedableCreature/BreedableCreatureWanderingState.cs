using UnityEngine;

public class BreedableCreatureWanderingState : BreedableCreatureState
{
    public BreedableCreatureWanderingState(StateMachine stateMachine, BreedableCreatureMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    //Run only once when Breedable Creature first enters the Wandering state.
    public override void Enter()
    {
        Debug.Log("Entered Wander State");
        movementComponent.handleBeginWander();
        base.Enter();
    }

    //Runs once per frame. Use function to handle core logic for idle
    public override void HandleInput()
    {
        //Wander
        movementComponent.wander();
    }

    //Runs once per frame. Use function to decide if we will be entering a new state.
    public override void LogicUpdate()
    {
        var displacementFromTarget = Vector3.ProjectOnPlane(movementComponent.targetPosition - movementComponent.transform.position, Vector3.up);
        if (displacementFromTarget.magnitude < BreedableCreatureMoveComponent.CONTINGENCY_DISTANCE)
        {
            stateMachine.ChangeState(movementComponent.idle);
        }

        base.LogicUpdate();
    }

    //Run only once when Breedable Creature first exits the Idle state.
    public override void Exit()
    {
    }
}
