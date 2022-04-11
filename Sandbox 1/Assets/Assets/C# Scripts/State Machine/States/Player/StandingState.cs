using UnityEngine;

public class StandingState : GroundedState
{
    private bool jump;

    public StandingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        base.Enter();
        jump = false;
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {
        base.HandleInput();
        jump = Input.GetButtonDown("Jump");
    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
    }

}
