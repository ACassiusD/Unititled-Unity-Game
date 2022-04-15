using UnityEngine;
using Polyperfect.Animals;
using Polyperfect.Common;

public class MountAnimatorComponent : MonoBehaviour
{
    IdleState[] idleStates;
    MovementState[] movementStates;
    AIState[] attackingStates;
    AIState[] deathStates;
    Animal_WanderScript wanderscript; //Get wanderscript and nav mesh agent, disable them in that order
    Animator mountAnimator;
    public float walkAnimationSpeed = 1;
    public float runAnimaitonSpeed = 2;

    void Awake()
    {
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

    public void setWalkingAnimation(float speed = 0)
    {
        ClearAnimation();

        foreach (var state in this.movementStates)
        {
            if (speed != 0)
            {
                SetAnimationBool(state.animationBool, true, speed);
            }
            else
            {
                SetAnimationBool(state.animationBool, true, walkAnimationSpeed);
            }

            break;
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

    public void ClearAnimation()
    {
        mountAnimator.speed = 1; 
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
}
