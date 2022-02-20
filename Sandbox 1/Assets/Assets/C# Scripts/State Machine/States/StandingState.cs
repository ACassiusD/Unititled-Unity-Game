﻿using UnityEngine;

public class StandingState : GroundedState
{
    private bool jump;
    private bool crouch;



    public StandingState(StateMachine stateMachine, MovementComponent moveComp) : base(stateMachine, moveComp)
    {
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        base.Enter();
        crouch = false;
        jump = false;
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {
        base.HandleInput();
        crouch = Input.GetButtonDown("Fire3");
        jump = Input.GetButtonDown("Jump");
    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (crouch)
        {
            stateMachine.ChangeState(movementComponent.ducking);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
    }

}