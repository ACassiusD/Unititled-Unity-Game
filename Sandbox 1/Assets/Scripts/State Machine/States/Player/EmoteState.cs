using UnityEngine;

public class EmoteState : PlayerState
{
    PlayerAnimator animator;
    private bool jump;
    private float horizontal = Input.GetAxisRaw("Horizontal");
    private float vertical = Input.GetAxisRaw("Vertical");
    
    public EmoteState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    public override void Enter()
    {
        animator.setEmoteAnimation();
        base.Enter();
    }

    public override void Exit()
    {
    }

    public override void HandleInput()
    {
        jump = Input.GetButtonDown("Jump");
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }


    public override void LogicUpdate()
    {

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        if (direction.magnitude >= 0.1f || jump)
        {
            stateMachine.ChangeState(movementComponent.standing);
        }
        //if (jump)
        //{
        //    stateMachine.ChangeState(movementComponent.jumping);
        //}
        base.LogicUpdate();
    }


}

