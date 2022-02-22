using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generic movement component class meant to be inherited from.
//Incudes states, functions and varaibles unique to the entites movement. 
public class MoveComponent : MonoBehaviour
{
    public float moveSpeed = 50f;
    public StateMachine movementSM;

    protected void Awake()
    {
        movementSM = new StateMachine();
    }

    protected void Start()
    {
        //characterController = GetComponent<CharacterController>();
        //standing = new StandingState(movementSM, this);
        //ducking = new DuckingState(movementSM, this);
        //jumping = new JumpingState(movementSM, this);
        //riding = new RidingState(movementSM, this);

        //if (characterController.isGrounded)
        //{
        //    movementSM.Initialize(standing);
        //}
        //else
        //{
        //    movementSM.Initialize(jumping);
        //}
    }

    protected void Update()
    {
        movementSM.CurrentState.HandleInput();
        movementSM.CurrentState.LogicUpdate();
    }
   

    public void ResetMoveParams()
    {
    }

}
