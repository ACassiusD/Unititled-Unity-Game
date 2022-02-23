using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountStandingState : MountState
{
    // Start is called before the first frame update
    public MountStandingState(StateMachine stateMachine, MountMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered MOUNT Standing state");
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


        base.LogicUpdate();
    }
}
