using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator mountAnimator;
    string runningBool = "isRunning";
    string idleBool = "isIdle";
    string walkingBool = "isWalking";
    string dancingBool = "isDancing";
    string jumpingBool = "isJumping";
    string fallingBool = "isFalling";

    public float walkAnimationSpeed = 1;
    public float runAnimaitonSpeed = 2;

    public void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            setIdleAnimation();
        }
        else if (Input.GetKeyDown("y"))
        {
            setWalkingAnimation();
        }
        else if (Input.GetKeyDown("u"))
        {
            setRunningAnimation();

        }
        else if (Input.GetKeyDown("i"))
        {
            setDanceAnimation();
        }
    }

    void Awake()
    {
        mountAnimator = this.GetComponent<Animator>();
    }

    public void setWalkingAnimation(float speed = 0)
    {
        ClearAnimation();
        SetAnimationBool(walkingBool, true, walkAnimationSpeed);
    }

    public void setRunningAnimation() { 
        ClearAnimation();
        SetAnimationBool(runningBool, true, runAnimaitonSpeed);
    }

    public void setIdleAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        SetAnimationBool(idleBool, true, animationSpeed);
    }

    public void setJumpingAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        SetAnimationBool(jumpingBool, true, animationSpeed);
    }
    public void setFallingAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        SetAnimationBool(fallingBool, true, animationSpeed);
    }

    public void setDanceAnimation(float animationSpeed = 0)
    {
        ClearAnimation();
        SetAnimationBool(dancingBool, true, animationSpeed);
    }

    public void ClearAnimation()
    {
        mountAnimator.speed = 1;
        SetAnimationBool(walkingBool, false, 0);
        SetAnimationBool(runningBool, false, 0);
        SetAnimationBool(idleBool, false, 0);
        SetAnimationBool(dancingBool, false, 0);
        SetAnimationBool(fallingBool, false, 0);
        SetAnimationBool(jumpingBool, false, 0);
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
