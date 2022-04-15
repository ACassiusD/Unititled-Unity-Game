using Polyperfect.Animals;
using UnityEngine;
using UnityEngine.AI;

public class Mount : MonoBehaviour, InteractableInterface //Mount class contains a movement component to move, and mount specific functions and params. 
{
    //TODO: CREATE MOUNTMoveComponent and states, use animatior to update them.
    public MountAnimatorComponent mountAnimator;   
    BetaCharacter playerScript;
    public MountMoveComponent moveComponent;
    public float ridingHeight = 5.4f;
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    public float dismountDistance = 8.0f;
    public bool isWandering = false;

    public void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    protected virtual void Start()
    {
        moveComponent = this.GetComponent<MountMoveComponent>();
        mountAnimator = this.GetComponent<MountAnimatorComponent>();

//        if (!moveComponent)
 //           Debug.LogError(this.name + " is missing a MountMoveComponent!");
 //       if (!mountAnimator)
 //           Debug.LogError(this.name + " is missing a MountAnimatorComponent!");

        if (!moveComponent.isBeingControlled)
            isWandering = true;

        onCreate();
    }

    protected void Update()
    {
        if (moveComponent.isBeingControlled)
        {
            updateAnimations();
            getCommandUpdates();
        }
    }

    private void updateAnimations()
    {
        if(mountAnimator != null)
        {
            mountAnimator.ClearAnimation();

            if (moveComponent.isBeingControlled)
            {
                if (moveComponent.isMoving)
                {
                    //Running
                    if (moveComponent.isRunning)
                    {
                        mountAnimator.setRunningAnimation();
                    }
                    else//walking
                    {
                        mountAnimator.setWalkingAnimation();
                    }
                }
            }
        }

    }

    public void interact()
    {
        Debug.Log("Interacted with mount");
        
        if (!playerScript.getIsRiding()) 
            mount();
        else
            dismount();
    }

    public void mount()
    {
        if(moveComponent.wanderscript != null)
        {
            moveComponent.wanderscript.enabled = false;
        }
        moveComponent.enabled = true;
        moveComponent.naveMeshAgent.enabled = false;
        gameObject.layer = 0;

        moveComponent.isBeingControlled = true;
        playerScript.setActiveMount(this);
        isWandering = false;
        playerScript.moveToMountedPosition();
    }

    public void dismount()
    {
        playerScript.setIsRiding(false);
        gameObject.layer = 8;

        moveComponent.naveMeshAgent.enabled = true;
        moveComponent.isBeingControlled = false;
        playerScript.DisMount(dismountDistance);
        isWandering = true;
        if(mountAnimator != null)
        {
             mountAnimator.ClearAnimation();

        }
        if (moveComponent.wanderscript != null)
        {
            moveComponent.wanderscript.resetOrigin();
            moveComponent.wanderscript.UpdateAI();
            moveComponent.wanderscript.enabled = true;
        }
        moveComponent.enabled = false;
        moveComponent.naveMeshAgent.enabled = true;
    }

    public virtual void attack()
    {
        Debug.Log(this.name + " Attacks!");
    }

    // Gets commands from player and responds
    public void getCommandUpdates()
    {
        bool attackKeyCaptured = Input.GetKeyDown("q");
        if (attackKeyCaptured)
        {
            attack();
        }
    }


}