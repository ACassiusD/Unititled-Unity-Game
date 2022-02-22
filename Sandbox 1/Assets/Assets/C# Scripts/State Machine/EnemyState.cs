public class EnemyState : State
{
    protected EnemyMovementComponent movementComponent;

    //Constructor
    protected EnemyState(StateMachine stateMachine, EnemyMovementComponent movementComponent) : base(stateMachine)
    {
        this.movementComponent = movementComponent;
    }
}
