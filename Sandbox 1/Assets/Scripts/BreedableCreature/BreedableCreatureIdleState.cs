using UnityEngine;

public class BreedableCreatureIdleState : BreedableCreatureState
{
    float waitTime;
    private float currentTime;

    public BreedableCreatureIdleState(StateMachine stateMachine, BreedableCreatureMoveComponent movementComponent) : base(stateMachine, movementComponent)
    {
    }

    //Run only once when Breedable Creature first enters the Idle state.
    public override void Enter()
    {

        waitTime = Random.Range(4f, 6f); //Decide how long we want to idle for
        Debug.Log("Entered Idle State. -- " + waitTime + "seconds");
        currentTime = waitTime;
        base.Enter();
    }

    //Runs once per frame. Use function to handle core logic for idle
    public override void HandleInput()
    {
        currentTime -= Time.deltaTime;
    }

    //Runs once per frame. Use function to decide if we will be entering a new state.
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (currentTime <= 0)
        {
            stateMachine.ChangeState(movementComponent.wandering);
        }
    }

    //Run only once when Breedable Creature first exits the Idle state.
    public override void Exit()
    {

    }
}
