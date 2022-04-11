public class MountState : State
{
    protected MountMoveComponent movementComponent;

    //Constructor
    protected MountState(StateMachine stateMachine, MountMoveComponent movementComponent) : base(stateMachine)
    {
        this.movementComponent = movementComponent;
    }
}