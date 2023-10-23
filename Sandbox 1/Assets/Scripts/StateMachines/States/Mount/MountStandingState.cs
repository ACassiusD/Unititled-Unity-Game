using UnityEngine;

//TODO: Need an idle state
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
        movementComponent.getMountScript().mountAnimator.ClearAnimation();
        if (movementComponent.isDebugging)
            Debug.Log("Mount Standing state");

        base.Enter();
    }

    //HANDLE STANDING MOVEMENT LOGIC
    public override void HandleInput()
    {
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.MoveMountViaInput();

        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();

        if (movementComponent.playerControls.Player.LeftControl.WasPressedThisFrame())
            movementComponent.toggleRun();

        //if idle
        if (!movementComponent.isMoving())
        {
            movementComponent.RegenerateStaminaMeter();
        }
        else
        {
            if (movementComponent.currentSpeed == movementComponent.walkSpeed)
            {
                movementComponent.RegenerateStaminaMeter();
            }
            else
            {
                movementComponent.ConsumeSprintMeter();
            }
        }
    }

    //DECIDE NEXT STATE
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!movementComponent.isBeingControlled)
            stateMachine.ChangeState(movementComponent.wandering);
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
            stateMachine.ChangeState(movementComponent.jumping);
    }

    //EXIT LOGIC
    public override void Exit()
    {
    }
}
