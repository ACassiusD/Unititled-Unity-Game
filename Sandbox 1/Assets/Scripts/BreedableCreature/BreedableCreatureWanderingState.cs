using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedableCreatureWanderingState : BreedableCreatureState
{
    public BreedableCreatureWanderingState(StateMachine stateMachine, BreedableCreatureMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    //ENTER LOGIC
    public override void Enter()
    {
        // movementComponent.wanderscript.enabled = true;
        //Debug.Log("Entered MOUNT WANDERING state");
        base.Enter();
    }

    //HANDLE STANDING MOVEMENT LOGIC
    public override void HandleInput()
    {

    }

    //DECIDE NEXT STATE
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //if (movementComponent.isBeingControlled)
        //{
        //    stateMachine.ChangeState(movementComponent.standing);
       // }
    }

    //EXIT LOGIC
    public override void Exit()
    {
        if (movementComponent.wanderscript != null)
        {
            movementComponent.wanderscript.enabled = false;
        }
    }
}
