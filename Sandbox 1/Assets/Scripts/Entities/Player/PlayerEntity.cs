using Polyperfect.Common;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    protected Vector3 spawnPosition;
    public CharacterController controller;

    //Components to control entity logic    
    //TODO: Finish melee attack with hitbox and everything.
        // TODO: Fix melee hitbox pushing around mountd player.
    //TODO: ADD Combat override Component / state machines.
    public PlayerMovementComponent playerMovementComponent;
    private PlayerStatsComponent playerStatsComponent;
    private PlayerCombatComponent playerCombatComponent; 
    public PlayerAnimator playerAnimator; //playerAnimatorComponent
    public InventoryHolder inventory; //playerInventoryComponent

    //Move to MountManager Comopnent eventually
    public Mount currentMount;

    //Move to UI manager component.
    public HealthBar healthBarScript;

    [SerializeField] private bool matchSurfaceRotation = true;
    [SerializeField] private int surfaceRotationSpeed = 20;

    //If true, this animal will rotate to match the terrain. Ensure you have set the layer of the terrain as 'Terrain'
    private void Awake()
    {
        //Get Components.
        inventory = this.GetComponent<InventoryHolder>();
        playerAnimator = GetComponent<PlayerAnimator>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        playerMovementComponent = this.GetComponent<PlayerMovementComponent>();
        playerStatsComponent = this.GetComponentInChildren<PlayerStatsComponent>();
        playerCombatComponent = this.GetComponentInChildren<PlayerCombatComponent>();

        //Initialize any components if needed. 
        playerCombatComponent.Initialize(playerStatsComponent);

        //Subscribe to component events.
        //Combatcomponents OnStunned event so we can do things like Stun with move component.
        playerCombatComponent.OnStunned += handleStun;

        //Match surface rotation to the terrain. 
        //TODO: Move to movement component? 
        if (matchSurfaceRotation && transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
        }

        if (!playerMovementComponent)
            Debug.LogError(this.name + " is missing a MoveComponent!");
        if (!healthBarScript)
            Debug.Log(this.name + " is missing a HealthBarScript!");
    }

    private void handleStun(float stunDuration)
    {
        playerMovementComponent.stunTimer = playerMovementComponent.stunDuration;
    }

    public void Update()
    {
        //Can't be handled in movementStateMachine since it has no access to this method.
        if (playerMovementComponent.isRunning)
            UpdateStaminaUI();

        SetControls();
    }

    public void DisMount(float dismountDistance = 10f)
    {
        playerMovementComponent.activeMount = null;
        this.transform.Translate(dismountDistance, 0, 0);
    }

    //TODO: Update to a state
    public void setIsRiding(bool passedVal)
    {
        playerMovementComponent.isRiding = passedVal;
    }

    public void setActiveMount(Mount mount)
    {
        playerMovementComponent.activeMount = mount;
        playerMovementComponent.isRiding = true;
    }

    public void moveToMountedPosition()
    {
        var activeMount = playerMovementComponent.activeMount;

        //Calculate where the rider needs to be positioned, then transform him to that position and rotation
        Vector3 ridingPositon = activeMount.transform.position;
        ridingPositon.y = ridingPositon.y + activeMount.ridingHeight;
        transform.position = ridingPositon;

        //Rotation
        transform.rotation = activeMount.transform.rotation;
    }

    /// <summary>
    /// Update floating healthbar in world space.
    /// </summary>
    //public void UpdateFloatingHealthBarUI()
    //{
    //    UIController.Instance.SetHealth(playerStatsComponent.currentHealth, playerStatsComponent.maxHealth);
    //}


    /// <summary>
    /// What happens when the entity's health reaches 0
    /// </summary>
    public void Feint()
    {
        playerCombatComponent.Feint();
    }

    /// <summary>
    /// Set the spawn position for the entity.
    /// </summary>
    public void SetSpawn()
    {
        this.spawnPosition = this.transform.position;
    }

    /// <summary>
    /// Repawn entity in set spawn position
    /// </summary>
    protected void Respawn()
    {

        if (this.spawnPosition.magnitude > 0)
        {
            this.transform.position = this.spawnPosition;
        }
    }

    protected void SetControls()
    {
        bool attackKeyCaptured = playerMovementComponent.playerControls.Player.P.WasPerformedThisFrame();
        bool attackKeyCaptured2 = playerMovementComponent.playerControls.Player.O.WasPerformedThisFrame();

        if (attackKeyCaptured)
        {
            Respawn();
        }
        if (attackKeyCaptured2)
        {
            SetSpawn();
        }
    }

    /// <summary>
    /// TODO: It might be good to move this into a player UI specific class, but its ok here for now.
    /// Updates the Stamina bar UI for the user. If the user is in control, users stamina values will be used.
    /// If the user is riding a mount, mounts stamina values will be used.
    /// </summary>
    public void UpdateStaminaUI()
    {
        float currentSprintTime = playerMovementComponent.sprintTimer;
        float currentSprintLimit = playerMovementComponent.sprintLimit;

        if (playerMovementComponent.isBeingControlled == false)
        {
            MountMoveComponent mountMoveComponent = playerMovementComponent.activeMount.GetComponent<MountMoveComponent>();
            currentSprintLimit = mountMoveComponent.sprintLimit;
            currentSprintTime = mountMoveComponent.sprintTimer;
        }
        UIController.Instance.SetStamina(currentSprintTime, currentSprintLimit);
    }

}
