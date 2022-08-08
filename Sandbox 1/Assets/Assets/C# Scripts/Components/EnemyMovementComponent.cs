using UnityEngine.AI;
using Polyperfect.Animals;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovementComponent : MoveComponent
{
    public EnemyStandingState standing;
    public EnemyChasingState chasing;
    public EnemyKnockbackState knockback;
    public NavMeshAgent naveMeshAgent;
    public Animal_WanderScript wanderscript;
    public Enemy enemyScript;

    void Start()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        getNavMesh();
        getWanderScript();
        standing = new EnemyStandingState(stateMachine, this);
        chasing = new EnemyChasingState(stateMachine, this);
        knockback = new EnemyKnockbackState(stateMachine, this);

        //Initialize state machine.
        stateMachine.Initialize(standing);
    }

    Animal_WanderScript getWanderScript()
    {
        if(wanderscript == null)
        {
            wanderscript = this.GetComponent<Animal_WanderScript>();
        }
        return wanderscript;
    }

    NavMeshAgent getNavMesh()
    {
        if (wanderscript == null)
        {
            naveMeshAgent = this.GetComponent<NavMeshAgent>();
        }
        return naveMeshAgent;
    }

    public Enemy getEnemyScript()
    {
        if(enemyScript == null)
        {
            enemyScript = this.GetComponent<Enemy>();
        }
        return enemyScript;
    }

    //Should be moved to entity script
    public void MoveTowardsTarget()
    {
        Vector3 directionTowardsPlayer = (target.transform.position - this.transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionTowardsPlayer.x, directionTowardsPlayer.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //this.transform.LookAt(this.target.transform);
        distanceFromTarget = Vector3.Distance(this.target.transform.position, this.transform.position);

        //Check if enemy is close enough to the player
        if (distanceFromTarget >= minDistanceFromTarget)
        {
            this.getEnemyScript().enemyAnimator.setRunningAnimation();
            this.isMoving = true;
            this.isRunning = true;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            this.getEnemyScript().enemyAnimator.setIdleAnimation();
            this.isMoving = false;
            this.isRunning = false;
        }
    } 
}
