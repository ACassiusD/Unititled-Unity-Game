using UnityEngine;

public class EmoteState : PlayerState
{
    PlayerAnimator animator;
    private bool jump;
    private float horizontal = 0;
    private float vertical = 0;

    public EmoteState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
        animator = movementComponent.getPlayerScript().playerAnimator;
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
        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();
        horizontal = movementComponent.playerControls.Player.Movement.ReadValue<Vector2>().x;//Input.GetAxisRaw("Horizontal");
        vertical = movementComponent.playerControls.Player.Movement.ReadValue<Vector2>().y;//= Input.GetAxisRaw("Vertical");
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

