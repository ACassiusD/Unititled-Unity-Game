using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountStandingState : MountState
{
    private bool jump;
    // Start is called before the first frame update
    public MountStandingState(StateMachine stateMachine, MountMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    //ENTER LOGIC
    public override void Enter()
    {
        Debug.Log("Entered MOUNT Standing state");
        base.Enter();
    }

    //HANDLE STANDING MOVEMENT LOGIC
    public override void HandleInput()
    {
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.MoveMountViaInput();
        jump = Input.GetButtonDown("Jump");
    }

    //DECIDE NEXT STATE
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!movementComponent.isBeingControlled)
        {
            stateMachine.ChangeState(movementComponent.wandering);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
    }

    //EXIT LOGIC
    public override void Exit()
    {
    }
}
