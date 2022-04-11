using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementComponent : MoveComponent
{
    public EnemyStandingState standing;
    public EnemyChasingState chasing;

    // Start is called before the first frame update
    void Start()
    {
        standing = new EnemyStandingState(stateMachine, this);
        chasing = new EnemyChasingState(stateMachine, this);
        //Initialize the state machine.
        stateMachine.Initialize(standing);
    }
}
