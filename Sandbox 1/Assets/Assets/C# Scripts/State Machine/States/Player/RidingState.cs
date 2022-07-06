using UnityEngine;

public class RidingState : PlayerState
{
    private bool grounded;
    private int jumpParam = Animator.StringToHash("Jump");
    private int landParam = Animator.StringToHash("Land");
    Vector3 velocity = new Vector3(); //Velocity/gravity force will increase when character is falling, until they become grounded.
    CharacterController controller;
    bool justEntered = false;
    [SerializeField]
    public bool isTest = true;
    PlayerAnimator animator;

    private bool jump;
    bool isGrounded = false;

    public RidingState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
        animator = movementComponent.getPlayerScript().animator;
    }

    public override void Enter()
    {
        animator.setRidingAnimation();
        if (movementComponent.isDebugging)
        {
            Debug.Log("Entered riding state");
        }
        movementComponent.EnabledSurfaceRotation();
        base.Enter();

    }
    public override void Exit()
    {
        movementComponent.activeMount.dismount();
        movementComponent.DisableSurfaceRotation();
    }

    public override void HandleInput()
    {
        movementComponent.RegenerateStamina();
        movementComponent.MoveToMountedPosition();
    }


    public override void LogicUpdate()
    {
        bool capturedKeyPress = Input.GetKeyDown(KeyCode.LeftControl);
        if (capturedKeyPress)
        {
            movementComponent.isRiding = false;
            stateMachine.ChangeState(movementComponent.standing);
            base.LogicUpdate();
        }
    }

}

