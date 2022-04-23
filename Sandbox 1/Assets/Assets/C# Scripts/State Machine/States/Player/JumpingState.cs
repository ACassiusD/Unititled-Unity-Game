﻿using UnityEngine;

public class JumpingState : PlayerState
{
    private int jumpParam = Animator.StringToHash("Jump");
    private int landParam = Animator.StringToHash("Land");
    CharacterController controller;
    bool justEntered = false;
    [SerializeField]
    public bool isTest = true;
    public bool isRiding = true;
    private bool jump;
    private float jumptimer = 0f;

    public JumpingState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        movementComponent.groundCheckTimer = movementComponent.groundCheckTime;
        //Debug.Log("Jump state time = " + movementComponent.jumpStateTime);
        jumptimer = movementComponent.jumpStateTime;
        //Debug.Log(jumptimer);
        if (movementComponent.isDebugging)
        {
            Debug.Log("Entered jumping state");
        }

        movementComponent.AddJumpVelocity();
        movementComponent.getPlayerScript().animator.setJumpingAnimation();
        //justEntered = true;
        base.Enter();
    }

    public override void Exit()
    {
        movementComponent.jumpCount = 0;
    }

    public override void HandleInput()
    {
        jump = Input.GetButtonDown("Jump");
        if (!justEntered)
        {
            movementComponent.ZeroYVelocityIfGrounded();
            movementComponent.addGravity();
            if (jump)
            {
                movementComponent.MidAirJump();
            }
        }
        movementComponent.AddVelocityAndMove();
        movementComponent.MovePlayerViaInput();
        jumptimer -= Time.deltaTime;
        movementComponent.groundCheckTimer -= Time.deltaTime;
        //Debug.Log(jumptimer);
        base.HandleInput();
    }


    public override void LogicUpdate()
    {
        if (movementComponent.isGrounded() && movementComponent.groundCheckTimer <= 0f)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        if (jumptimer <= 0f)
        {
            //Debug.Log("Timer Ended");
            stateMachine.ChangeState(movementComponent.falling);
        }
        base.LogicUpdate();
    }


}

