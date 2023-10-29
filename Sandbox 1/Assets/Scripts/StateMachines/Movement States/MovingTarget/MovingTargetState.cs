//Since all MovingTarget states need a reference to the movingTargetMovementComponent.
//We create this derrived class which the rest will use as a basis.
public class MovingTargetState : State
{
    protected MovingTargetMovementComponent movementComponent;

    //Constructor
    protected MovingTargetState(StateMachine stateMachine, MovingTargetMovementComponent movementComponent) : base(stateMachine)
    {
        this.movementComponent = movementComponent;
    }
}
