using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : GroundedState
{
    PlayerAnimator animator;
    private bool jump;

    public MovingState(StateMachine stateMachine, PlayerMovementComponent moveComp) : base(stateMachine, moveComp)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    //While entering, calculate all relevent variables and other beginning methods here.
    public override void Enter()
    {
        if (movementComponent.isDebugging)
        {
            Debug.Log("GROUNDED MOVING");
        }
    }

    //Hypotetically, we can only jump and shoot while standing, but not ducking or crouching. So cache these variables here instead of the sub class, grounded state
    public override void HandleInput()
    {

        base.HandleInput();
        if (movementComponent.isRunning)
        { 
            animator.setRunningAnimation();
        }
        else
        {           
            animator.setWalkingAnimation();
            movementComponent.RegenerateStamina();
        }

        jump = movementComponent.playerControls.Player.Jump.WasPerformedThisFrame();

    }

    //Decide the next state for the character.
    public override void LogicUpdate()
    {
        //Returns 0, -1 or 1 for corrosponding direction
        float horizontal = movementComponent.playerControls.Player.Movement.ReadValue<Vector2>().x;
        float vertical = movementComponent.playerControls.Player.Movement.ReadValue<Vector2>().y;
        bool isMoving = false; 
        //Calcuate the Vector3 direction, and normalize it to a lenght of 1 unit (just get the direction we want to wak in p much)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //If we picked up a movement input
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
        }

        base.LogicUpdate();

        if (movementComponent.stunTimer != 0)
        {
            stateMachine.ChangeState(movementComponent.stun);
        }
        if (movementComponent.isRiding)
        {
            stateMachine.ChangeState(movementComponent.riding);
        }
        else if (jump && movementComponent.jumpCount < movementComponent.maxJumps)
        {
            stateMachine.ChangeState(movementComponent.jumping);
        }
        else if(isMoving){
            this.stateMachine.ChangeState(movementComponent.standing);
        }
        else if (!movementComponent.IsGrounded())
        {
             this.stateMachine.ChangeState(movementComponent.falling);   
        }
    }
}
