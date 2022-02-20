using UnityEngine;

public class RidingState : State
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

    public RidingState(StateMachine stateMachine, MovementComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    public override void Enter()
    {
        Debug.Log("Entered riding state");
        base.Enter();

    }
    public override void Exit()
    {
        //movementComponent.jumpCount = 0;
    }

    public override void HandleInput()
    {
        movementComponent.moveToMountedPosition();
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

