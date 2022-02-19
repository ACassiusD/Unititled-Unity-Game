using UnityEngine;

public class DuckingState : GroundedState
{
    private bool belowCeiling;
    private bool crouchHeld;

    public DuckingState(StateMachine stateMachine, MovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

}
