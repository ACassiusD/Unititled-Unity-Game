public class PlayerState : State
{
    protected PlayerMovementComponent movementComponent;

    //Constructor
    protected PlayerState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine)
    {
        this.movementComponent = movementComponent;
    }
}
