public class DuckingState : GroundedState
{
    private bool belowCeiling;
    private bool crouchHeld;

    public DuckingState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

}
