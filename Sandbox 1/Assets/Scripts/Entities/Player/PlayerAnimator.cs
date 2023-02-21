using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public bool debug = false;
    Animator mountAnimator;
    //1. First, reprisent the animation bool in code.
    string runningBool = "isSprinting";
    string idleBool = "isIdle";
    string walkingBool = "isWalking";
    string emoteBool = "isEmote";
    string jumpingBool = "isJumping";
    string fallingBool = "isFalling";
    string ridingBool = "isRiding";
    string stunnedBool = "isStunned";
    public float walkAnimationSpeed = 1;
    public float runAnimaitonSpeed = 2;

    void Awake()
    {
        mountAnimator = this.GetComponent<Animator>();
    }

    public void restartJumpingAnimation()
    {
        mountAnimator.Play("Jumping", -1, 0f);
    }

    //2. Second, create the function to set the bool
    public void setWalkingAnimation(float speed = 0)
    {
        if(debug)
        debugAnimations("WALKING BOOL");
        ClearAnimation();
        SetAnimationBool(walkingBool, true, walkAnimationSpeed);
    }

    public void setRunningAnimation() {
        debugAnimations("RUNNING BOOL");
        ClearAnimation();
        SetAnimationBool(runningBool, true, runAnimaitonSpeed);
    }

    public void setIdleAnimation(float animationSpeed = 0)
    {
        debugAnimations("IDLE BOOL");
        ClearAnimation();
        SetAnimationBool(idleBool, true, animationSpeed);
    }

    public void setJumpingAnimation(float animationSpeed = 0)
    {
        debugAnimations("JUMPING BOOL");
        ClearAnimation();
        SetAnimationBool(jumpingBool, true, animationSpeed);
    }
    public void setFallingAnimation(float animationSpeed = 0)
    {
        debugAnimations("FALLING BOOL");
        ClearAnimation();
        SetAnimationBool(fallingBool, true, animationSpeed);
    }

    public void setEmoteAnimation(float animationSpeed = 0)
    {
        debugAnimations("Emote BOOL");
        ClearAnimation();
        SetAnimationBool(emoteBool, true, animationSpeed);
    }

    public void setRidingAnimation(float animationSpeed = 0)
    {
        debugAnimations("RIDING BOOL");
        ClearAnimation();
        SetAnimationBool(ridingBool, true, animationSpeed);
    }

    public void setStunnedAnimation(float animationSpeed = 0)
    {
        debugAnimations("STUNNED BOOL");
        ClearAnimation();
        SetAnimationBool(stunnedBool, true, animationSpeed);
    }

    public void ClearAnimation()
    {
        mountAnimator.speed = 1;
        SetAnimationBool(walkingBool, false, 0);
        SetAnimationBool(runningBool, false, 0);
        SetAnimationBool(idleBool, false, 0);
        SetAnimationBool(emoteBool, false, 0);
        SetAnimationBool(fallingBool, false, 0);
        SetAnimationBool(jumpingBool, false, 0);
        SetAnimationBool(ridingBool, false, 0);
        SetAnimationBool(stunnedBool, false, 0);
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

    void debugAnimations(string message)
    {
        if (debug)
        {
            Debug.Log(message);
        }
    }
}
