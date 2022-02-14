using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
using Polyperfect.Common;

public class Enemy : Entity
{
    Animator animator;
    Animal_WanderScript wanderscript;
    IdleState[] idleStates;
    MovementState[] movementStates;
    AIState[] attackingStates;
    private AIState[] deathStates;
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    public float distanceToPlayer = 0;
    public float minDistanceFromPlayer = 8;
    public bool isWandering = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isWandering)
        {
            this.GetComponent<NavMeshAgent>().enabled = false;
        }
        isMoving = false;
        isControllable = false;
        initAnimationTools();

        base.Start();
    }

    private void initAnimationTools()
    {
        animator = this.GetComponent<Animator>();
        wanderscript = this.GetComponent<Animal_WanderScript>();
        if (wanderscript)
        {
            idleStates = wanderscript.idleStates;
            movementStates = wanderscript.movementStates;
            attackingStates = wanderscript.attackingStates;
            deathStates = wanderscript.deathStates;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Look at player
        this.transform.LookAt(playerReference.transform);
       // MoveTowardsPlayer();
        base.Update();
    }

    //Should be moved to entity script
    void MoveTowardsPlayer()
    {
        distanceToPlayer = Vector3.Distance(playerReference.transform.position, this.transform.position);

        //Check if enemy is close enough to the player
        if(distanceToPlayer <= minDistanceFromPlayer)
        {
            if(isMoving != false)
            {
                isMoving = false;
                setIdleAnimation();
            }
            return;

        }

        if (isMoving == false)
        {
            isMoving = true;
            setRunningAnimation();
        }

        float step = speed * Time.deltaTime; // calculate distance to move
                                             //Calculate gravity
        
        Vector3 MoveTowardsPlayer = Vector3.MoveTowards(transform.position, playerReference.transform.position, step);
        MoveTowardsPlayer.y = (Physics.gravity.y * gravityScale);
        transform.position = MoveTowardsPlayer;
    }

    public void ClearAnimation()
    {
        foreach (var item in this.idleStates)
            SetAnimationBool(item.animationBool, false, 0);
        foreach (var item in this.movementStates)
            SetAnimationBool(item.animationBool, false, 0);
        foreach (var item in this.attackingStates)
            SetAnimationBool(item.animationBool, false, 0);
        foreach (var item in this.deathStates)
            SetAnimationBool(item.animationBool, false, 0);
    }

    void SetAnimationBool(string parameterName, bool value, float speed)
    {
        if (speed != 0)
        {
            animator.speed = speed;
        }

        if (!string.IsNullOrEmpty(parameterName))
        {
            animator.SetBool(parameterName, value);
        }
    }

    //Attempts to play a unique running animation, if no running animation exists, use walking animation with running animation speed.
    public bool setRunningAnimation()
    {
        ClearAnimation();
        foreach (var state in this.movementStates)
        {
            if (state.animationBool == "isRunning")
            {
                SetAnimationBool(state.animationBool, true, runAnimaitonSpeed);
                return true;
            }
        }
        setWalkingAnimation(runAnimaitonSpeed);
        return false;
    }

    public void setWalkingAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        if (animationSpeed == 0)
            animationSpeed = walkAnimationSpeed;

        foreach (var state in this.movementStates)
        {
            SetAnimationBool(state.animationBool, true, animationSpeed);
            break;
        }
    }

    public void setIdleAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        if (animationSpeed == 0)
            animationSpeed = walkAnimationSpeed;

        foreach (var idleState in this.idleStates)
        {
            SetAnimationBool(idleState.animationBool, true, animationSpeed);
            break;
        }
    }

}
