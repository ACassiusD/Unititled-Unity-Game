using UnityEngine;
using UnityEngine.Events;

public class Mount : MonoBehaviour, IInteractable //Mount class contains a movement component to move, and mount specific functions and params. 
{
    public MountAnimatorComponent mountAnimator;
    public MountMoveComponent moveComponent;
    public BetaCharacter owner = null;
    BetaCharacter playerScript;
    public float ridingHeight = 5.4f;
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    public float dismountDistance = 8.0f;
    public bool isWandering = false;
    public bool tamed = true;


    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    protected virtual void Start()
    {
        moveComponent = this.GetComponent<MountMoveComponent>();
        mountAnimator = this.GetComponent<MountAnimatorComponent>();

        if (!moveComponent.isBeingControlled)
            isWandering = true;

        onCreate();
    }

    protected void Update()
    {
        if (moveComponent.isBeingControlled)
            updateAnimations();
    }

    private void updateAnimations()
    {
        if(mountAnimator != null)
        {
            mountAnimator.ClearAnimation();

            if (moveComponent.isBeingControlled)
            {
                if (moveComponent.isMoving() == true)
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

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        if (!playerScript.getIsRiding()) 
            mount();
        else
            dismount();

        interactSuccessful = true;
    }

    public void mount()
    {
        playerScript.setIsRiding(true);
        if (moveComponent.wanderscript != null)
        {
            moveComponent.wanderscript.enabled = false;
        }

        moveComponent.naveMeshAgent.enabled = false;
        gameObject.layer = 12;

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
        moveComponent.naveMeshAgent.enabled = true;
    }

    public virtual void basicAttack()
    {
        Debug.Log(this.name + " Basic Attacks!");
    }

    public virtual void specialAttack()
    {
        Debug.Log(this.name + " Special Attacks!!!");
    }

    public void UpdateTameStatus(bool tamedStats, BetaCharacter owner)
    {
        tamed = tamedStats;
        this.owner = owner;
    }

    public void clearTamedStatus()
    {
        tamed = false;
        owner = null;
    }

    public BetaCharacter getOwner()
    {
        return owner;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
