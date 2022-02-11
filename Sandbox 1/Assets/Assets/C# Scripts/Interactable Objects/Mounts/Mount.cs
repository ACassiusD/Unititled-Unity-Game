using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
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
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    protected float walkSpeed = 50;
    protected float runSpeed = 200;
    public bool isRunning = false;
    public float dismountDistance = 8.0f;


    protected virtual void Start()
    {
        base.Start();
        onCreate();
    }

    public override void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
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
            return;
        
        getCommandUpdates();
        MoveCharacterController();
        UpdateAnimation();
        toggleRun();
    }

    public void interact()
    {
        Debug.Log("Interacted with mount");
        
        //Character isn't riding. Call default movement
        if (!playerScript.isRiding) 
            mount();
        else
            dismount();
    }

    public void mount()
    {
        gameObject.layer = 0;
        var idleStates = wanderscript.idleStates;
        wanderscript.enabled = false;
        naveMeshAgent.enabled = false;
        isBeingControlled = true;
        playerScript.setActiveMount(this);
    }

    public void dismount()
    {
        gameObject.layer = 8;
        mountAnimator.speed = 1;
        wanderscript.resetOrigin();
        wanderscript.UpdateAI();
        naveMeshAgent.enabled = true;
        wanderscript.enabled = true;
        isBeingControlled = false;
        playerScript.unMount();
        playerScript.transform.Translate(dismountDistance, 0, 0);
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
            mountAnimator.speed = speed;
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

        ClearAnimation();

        if (isMoving)
        {
            //Running
            if (isRunning)
            {
                speed = runSpeed;
                setRunningAnimation();
            }
            else//walking
            {
                speed = walkSpeed;
                setWalkingAnimation();
            }
        }
    }

    public void setWalkingAnimation(float animationSpeed = 0)
    {
        if (animationSpeed == 0)
            animationSpeed = walkAnimationSpeed;
        
        foreach (var state in this.movementStates)
        {
            SetAnimationBool(state.animationBool, true, animationSpeed);
            break;
        }
    }

    //Attempts to play a unique running animation, if no running animation exists, use walking animation with running animation speed.
    public bool setRunningAnimation()
    {
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

    public void toggleRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isRunning ? isRunning = false : isRunning = true);
        }

    }

    //Gets commands from player and responds
    public void getCommandUpdates()
    {
        bool attackKeyCaptured = Input.GetKeyDown("q");
        if (attackKeyCaptured)
        {
            attack();
        }
    }

    public override void attack()
    {
        Debug.Log(this.name + " Attacks!");
    }
}