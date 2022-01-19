using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyperfect.Animals;
using UnityEngine.AI;
using Polyperfect.Common;

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

    public override void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        speed = 150;
        gravityScale = .25f;
        jumpForce = 70f;
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
        //Set walking animaiton.
        foreach (var idleState in idleStates)
        {
            ClearAnimatorBools();
            TrySetBool(idleState.animationBool, true);
            break;
        }
    }

    public void dismount()
    {
        wanderscript.resetOrigin();
        wanderscript.UpdateAI();
        naveMeshAgent.enabled = true;
        wanderscript.enabled = true;
        isBeingControlled = false;
        playerScript.unMount();
    }

    public void ClearAnimatorBools()
    {
        foreach (var item in idleStates)
            TrySetBool(item.animationBool, false);
        foreach (var item in movementStates)
            TrySetBool(item.animationBool, false);
        foreach (var item in attackingStates)
            TrySetBool(item.animationBool, false);
        foreach (var item in deathStates)
            TrySetBool(item.animationBool, false);
    }

    void TrySetBool(string parameterName, bool value)
    {
        if (!string.IsNullOrEmpty(parameterName))
        {
                mountAnimator.SetBool(parameterName, value);
        }
    }
}