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
    public int runningSpeed = 80;
    public float walkAnimationSpeed = 2;
    public static int mountSpeed = 99; // should be moved to individual mount classes for unique values.
    public int gorillaRunSpeed = 115;
    public int gorillaWalkSpeed = 50;
    public int lionRunSpeed = 50;
    public int lionWalkSpeed = 100;

    protected virtual void Start()
    {
        base.Start();
        onCreate();
    }

    public override void onCreate()
    {
        if (this.gameObject.name == "TESTER"){
            Debug.Log("test");
        }
        speed = mountSpeed;
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

        if (isWalking)
        {

            if (speed >= runningSpeed)
            {
                if (this.gameObject.name == "Lion")
                {
                    speed = lionRunSpeed;
                }
                if(this.gameObject.name == "Gorilla")
                {
                    speed = gorillaRunSpeed;
                }
                speed = mountSpeed;
                //Set Running animaiton.
                foreach (var state in this.movementStates)
                {
                    if(state.animationBool == "isRunning")
                    {
                        TrySetBool(state.animationBool, true, walkAnimationSpeed);
                        break;
                    }
                }
            }

            //Set walking animaiton.
            foreach (var state in this.movementStates)
            {
                if (this.gameObject.name == "Lion")
                {
                    speed = lionWalkSpeed;
                }
                if (this.gameObject.name == "Gorilla")
                {
                    speed = gorillaWalkSpeed;
                }
                TrySetBool(state.animationBool, true, walkAnimationSpeed);
                break;
            }
        }
    }
}