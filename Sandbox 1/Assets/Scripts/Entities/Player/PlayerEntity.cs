using Polyperfect.Common;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IDamageable
{
    public CharacterController controller;
    private GameObject[] tamedMounts; //Create MountCollection() Class
    Mount currentMount;
    public PlayerMovementComponent playerMovementComponent;
    HealthBar healthBarScript;
    public int currentHealth = 500;
    public int maxHealth = 500;
    public bool inHitStun = false;
    public bool matchSurfaceRotation = true;
    public int surfaceRotationSpeed = 20;
    public PlayerAnimator animator;
    protected Vector3 spawnPosition;
    public InventoryHolder inventory;
    
    //If true, this animal will rotate to match the terrain. Ensure you have set the layer of the terrain as 'Terrain'

    private void Awake()
    {
        inventory = this.GetComponent<InventoryHolder>();
        animator = GetComponent<PlayerAnimator>();   
        
        //Match surface rotation to the terrain
        if (matchSurfaceRotation && transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
        }
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        playerMovementComponent = this.GetComponent<PlayerMovementComponent>();
        
        if (!playerMovementComponent)
            Debug.LogError(this.name + " is missing a MoveComponent!");
        if (!healthBarScript)
            Debug.Log(this.name + " is missing a HealthBarScript!");
    }

    public void Update()
    {
        if (playerMovementComponent.isRunning)
        {
            UpdateStaminaUI();
        }
        SetControls();
    }

    public void onCreate()
    {
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }


    public void DisMount(float dismountDistance = 10f)
    {
        playerMovementComponent.activeMount = null;
        this.transform.Translate(dismountDistance, 0, 0);
    }


    //Text function for testing item pick up
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            //inventory.AddItem(other.GetComponent<Item>());
        }
    }
    
    public bool getIsRiding()
    {
        return playerMovementComponent.isRiding;
    }

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

    public int receiveDamage(int damageAmount, int knockBackForce, Vector3 direction = new Vector3())
    {
        currentHealth -= damageAmount;
        updateHealthBar();
        if (currentHealth <= 0)
        {
            feint();
            return 0;
        }
        //Knockback(knockBackForce);
        playerMovementComponent.stunTimer = playerMovementComponent.stunDuration;
        return currentHealth;
    }

    public void Knockback(float knockBackForce)
    {
        inHitStun = true;

        //Add Impact
        Vector3 direction = this.transform.forward * -1; //Need to make this direction
        Vector3 up = this.transform.up;
        up.Normalize();
        direction.Normalize();
        direction.y = up.y;
        var impact = Vector3.zero;
        impact += direction.normalized * knockBackForce;

        //Apply vector to object
        playerMovementComponent.characterController.Move(impact * Time.deltaTime);
    }

    //Update floating healthbar in world space.
    public void updateHealthBar()
    {
        if(healthBarScript != null)
        {
            healthBarScript.setHealth(currentHealth, maxHealth);
        }
        UIController.Instance.setHealth(currentHealth, maxHealth);
    }

    //Kill/death command, despawn and drop loot.
    public void feint()
    {
        //Debug.Log("oof, you died");
        //Destroy(gameObject);
    }

    public void SetSpawn()
    {
        this.spawnPosition = this.transform.position;
    }

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
            fullHeal();
            SetSpawn();
        }
    }   
    
    protected void fullHeal()
    {
        currentHealth = maxHealth;
        updateHealthBar();
        playerMovementComponent.sprintTimer = playerMovementComponent.sprintLimit;
        UpdateStaminaUI();
    }

    /// <summary>
    /// Updates the Stamina bar UI for the user. If the user is in control, users stamina values will be used.
    /// If the user is riding a mount, mounts stamina values will be used.
    /// </summary>
    public void UpdateStaminaUI()
    {
        float currentSprintTime = playerMovementComponent.sprintTimer;
        float currentSprintLimit = playerMovementComponent.sprintLimit;

        if(playerMovementComponent.isBeingControlled == false)
        {
            MountMoveComponent mountMoveComponent = playerMovementComponent.activeMount.GetComponent<MountMoveComponent>();
            currentSprintLimit = mountMoveComponent.sprintLimit;
            currentSprintTime = mountMoveComponent.sprintTimer;
        }
        UIController.Instance.setStamina(currentSprintTime, currentSprintLimit);
    }

}
