public class GroundedState : PlayerState
{
    public GroundedState(StateMachine stateMachine, PlayerMovementComponent moveComponent) : base(stateMachine, moveComponent)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Entered the grounded state!");
    }

    public override void Exit()
    {
        base.Exit();
    }

    //Grounded state gets the input for move direction only.
    public override void HandleInput()
    {
        base.HandleInput();
        movementComponent.ZeroYVelocityIfGrounded();
        movementComponent.addGravity();
        movementComponent.AddVelocityAndMove();
        movementComponent.MovePlayerViaInput();

        if (movementComponent.playerControls.Player.LeftControl.WasPressedThisFrame())
        {
            movementComponent.toggleRun();
        }
    }

}

