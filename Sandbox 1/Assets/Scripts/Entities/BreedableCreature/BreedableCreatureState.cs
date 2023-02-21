
public class BreedableCreatureState : State
{
    protected BreedableCreatureMoveComponent movementComponent;

    //Constructor
    protected BreedableCreatureState(StateMachine stateMachine, BreedableCreatureMoveComponent movementComponent) : base(stateMachine)
    {
        this.movementComponent = movementComponent;
    }
}