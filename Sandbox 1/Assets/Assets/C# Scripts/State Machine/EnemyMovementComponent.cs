using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementComponent : MoveComponent
{
    public Transform playerTransform;
    public CharacterController characterController;
    public EnemyStandingState standing;
    public EnemyChasingState chasing;
    public float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        standing = new EnemyStandingState(movementSM, this);
        chasing = new EnemyChasingState(movementSM, this);
        characterController = GetComponent<CharacterController>();
        //Initialize the state machine.
        movementSM.Initialize(standing);
    }

    //Should be moved to entity script
    public void MoveTowardsPlayer()
    {
        float step = moveSpeed * Time.deltaTime; // calculate distance to move
        this.transform.LookAt(playerTransform);
        Vector3 MoveTowardsPlayer = Vector3.MoveTowards(transform.position, playerTransform.position, step);
        //MoveTowardsPlayer.y = (Physics.gravity.y * gravityScale); //Calculate gravity
        transform.position = MoveTowardsPlayer;
    }

    public float getDistanceFromPlayer()
    {
        distanceFromPlayer = Vector3.Distance(playerTransform.position, characterController.transform.position);
        return distanceFromPlayer;
    }

    public bool IsInRangeOfPlayer()
    {
        if (getDistanceFromPlayer() < 100)
        {
            return true;
        }
        else{
            return false;
        }
    }
}
