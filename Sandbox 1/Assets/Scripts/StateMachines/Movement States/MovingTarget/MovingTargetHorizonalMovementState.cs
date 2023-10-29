using UnityEngine;

public class MovingTargetHorizonalMovementState : MovingTargetState
{
    public MovingTargetHorizonalMovementState(StateMachine stateMachine, MovingTargetMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
        this.movementComponent = movementComponent;
    }

    public override void Enter()
    {

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        movementComponent.Oscillate();
    }

    //Get next state
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
