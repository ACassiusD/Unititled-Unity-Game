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

    private bool jump;
    bool isGrounded = false;

    public RidingState(StateMachine stateMachine, PlayerMovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered riding state");
        base.Enter();

    }
    public override void Exit()
    {
        movementComponent.activeMount.dismount();
    }

    public override void HandleInput()
    {
        movementComponent.MoveToMountedPosition();
    }


    public override void LogicUpdate()
    {
        bool capturedKeyPress = Input.GetKeyDown("e") || Input.GetMouseButtonDown(0);
        if (capturedKeyPress)
        {
            movementComponent.isRiding = false;
            stateMachine.ChangeState(movementComponent.standing);
            base.LogicUpdate();
        }
    }

}

