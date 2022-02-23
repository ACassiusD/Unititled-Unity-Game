using Polyperfect.Animals;
using UnityEngine;
using UnityEngine.AI;

public class Mount : MonoBehaviour //Mount class contains a movement component to move, and mount specific functions and params. 
{
    //TODO: CREATE MOUNTMoveComponent and states, use animatior to update them.
    Animal_WanderScript wanderscript;
    MountAnimatorComponent mountAnimator;
    BetaCharacter playerScript;
    MovementComponent moveComponent;
    NavMeshAgent naveMeshAgent;
    public float ridingHeight = 5.4f;
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    protected float walkSpeed = 50;
    protected float runSpeed = 200;
    public bool isRunning = false;
    public float dismountDistance = 8.0f;
    public bool isBeingControlled = false;
    public bool isWandering = false;

    protected virtual void Start()
    {
        wanderscript = this.GetComponent<Animal_WanderScript>();
        if (!isBeingControlled)
        {
            isWandering = true;
        }
        onCreate();
    }

    public void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        naveMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    public void interact()
    {
        Debug.Log("Interacted with mount");
        
        //Character isn't riding. Call default movement
        if (!playerScript.getIsRiding()) 
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
        isWandering = false;
    }

    public void dismount()
    {
        playerScript.setIsRiding(false);
        gameObject.layer = 8;
        wanderscript.resetOrigin();
        wanderscript.UpdateAI();
        naveMeshAgent.enabled = true;
        wanderscript.enabled = true;
        isBeingControlled = false;
        playerScript.DisMount(dismountDistance);
        isWandering = true;
    }


    //public void toggleRun()
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftShift))
    //    {
    //        if (isRunning ? isRunning = false : isRunning = true);
    //    }

    //}
    //public void attack()
    //{
    //    Debug.Log(this.name + " Attacks!");
    //}

    //Gets commands from player and responds
    //public void getCommandUpdates()
    //{
    //    bool attackKeyCaptured = Input.GetKeyDown("q");
    //    if (attackKeyCaptured)
    //    {
    //        attack();
    //    }
    //}


}