using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyperfect.Animals;
using UnityEngine.AI;
using Polyperfect.Common;

//Mount class will probably just keep animaiton functions and stats + abilities will be moved to animal
public class Mount : Animal
{
    PlayerCharacter playerScript;
    //Get wanderscript and nav mesh agent, disable them in that order
    Animal_WanderScript wanderscript;
    NavMeshAgent naveMeshAgent;
    Animator mountAnimator;
    IdleState[] idleStates;
    MovementState[] movementStates;
    AIState[] attackingStates;
    private AIState[] deathStates;
    public float ridingHeight = 5.4f;
    public float walkAnimationSpeed = 2;
    public int walkSpeed = 50;
    public int runSpeed = 100;
    public bool isRunning = false;


    protected virtual void Start()
    {
        base.Start();
        onCreate();
    }

    public override void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        gravityScale = .25f;
        naveMeshAgent = this.GetComponent<NavMeshAgent>();
        wanderscript = this.GetComponent<Animal_WanderScript>();
        mountAnimator = this.GetComponent<Animator>();
        if (wanderscript)
        {
            idleStates = wanderscript.idleStates;
            movementStates = wanderscript.movementStates;
            attackingStates = wanderscript.attackingStates;
            deathStates = wanderscript.deathStates;
}
    }

    //Update is called once per frame
    protected override void Update()
    {
        if (!isBeingControlled)
        {
            return;
        }
        MoveCharacterController();
        UpdateAnimation();
    }

    public void interact()
    {
        Debug.Log("Interacted with mount");

        if (!playerScript.isRiding) //Character isn't riding. Call default movement
        {
            mount();
        }
        else
        {
            dismount();
        }
    }

    public void mount()
    {
        var idleStates = wanderscript.idleStates;
        wanderscript.enabled = false;
        naveMeshAgent.enabled = false;
        isBeingControlled = true;
        playerScript.setActiveMount(this);
    }

    public void dismount()
    {
        mountAnimator.speed = 1;
        wanderscript.resetOrigin();
        wanderscript.UpdateAI();
        naveMeshAgent.enabled = true;
        wanderscript.enabled = true;
        isBeingControlled = false;
        playerScript.unMount();
    }

    public void ClearAnimatorBools()
    {
        foreach (var item in this.idleStates)
            TrySetBool(item.animationBool, false, 0);
        foreach (var item in this.movementStates)
            TrySetBool(item.animationBool, false, 0);
        foreach (var item in this.attackingStates)
            TrySetBool(item.animationBool, false, 0);
        foreach (var item in this.deathStates)
            TrySetBool(item.animationBool, false, 0);
    }

    void TrySetBool(string parameterName, bool value, float speed)
    {
        if(speed != 0)
        {
            mountAnimator.speed = walkAnimationSpeed;
        }

        if (!string.IsNullOrEmpty(parameterName))
        {
                mountAnimator.SetBool(parameterName, value);
        }
    }

    private void UpdateAnimation()
    {
        if (!isBeingControlled)
        {
            return;
        }

        ClearAnimatorBools();

        if (isMoving)
        {
            //Running
            if (isRunning)
            {
                speed = runSpeed;
                if(setRunningAnimation() == false)
                {
                    setWalkingAnimation();
                }
            }
            else//walking
            {
                speed = walkSpeed;
                setWalkingAnimation();
            }
        }
    }

    public void setWalkingAnimation()
    {
        foreach (var state in this.movementStates)
        {
            TrySetBool(state.animationBool, true, walkAnimationSpeed);
            break;
        }
    }

    public bool setRunningAnimation()
    {
        foreach (var state in this.movementStates)
        {
            if (state.animationBool == "isRunning")
            {
                TrySetBool(state.animationBool, true, walkAnimationSpeed);
                return true;
            }
        }
        return false;
    }
}