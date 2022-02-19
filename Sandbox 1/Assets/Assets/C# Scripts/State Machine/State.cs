//REMOVE MONOBEHAVIOR INHERITANCE - only using for inspector variables
using UnityEngine;
public abstract class State
{
    protected StateMachine stateMachine;
    protected MovementComponent movementComponent;


    //Constructorc
    protected State(StateMachine stateMachine, MovementComponent movementComponent)
    {
        this.stateMachine = stateMachine;
        this.movementComponent = movementComponent;
    }

    //This is where you enter the state and do things you only need to do once when you first enter the state.
    public virtual void Enter()
    {
    }

    //Used to cache inputs and values
    public virtual void HandleInput()
    {

    }

    //Logic, move character controllers, make decisions, etc.
    public virtual void LogicUpdate()
    {

    }

    //Similar to the Entry, this is where you do any clean-ups you only need to do once before the state changes.
    public virtual void Exit()
    {

    }

    //Used to update physics if we have a ridged body
    //public virtual void PhysicsUpdate()
    //{

    //}
}