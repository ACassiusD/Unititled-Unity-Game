using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountMoveComponent : MoveComponent
{
    public bool isBeingControlled = false;
    public MountStandingState standing;
    Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded.

    private void Start()
    {
        standing = new MountStandingState(stateMachine, this); //Mount can be controlled by inputs if being controlled. or wander if not being controlled
         //Mount can jump if being controlled
        stateMachine.Initialize(standing);
    }
}
