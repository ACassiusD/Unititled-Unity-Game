using Polyperfect.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaCharacter : MonoBehaviour, IDamageable
{
    public CharacterController controller;
    private GameObject[] tamedMounts; //Create MountCollection() Class
    Mount currentMount;
    public PlayerMovementComponent movementComponent;
    HealthBar healthBarScript;
    public int currentHealth = 500;
    public int maxHealth = 500;
    public bool inHitStun = false;
    public bool matchSurfaceRotation = true;
    public int surfaceRotationSpeed = 20;
    public PlayerAnimator animator;

    private void Awake()
    {
        animator = GetComponent<PlayerAnimator>();   

        if (matchSurfaceRotation && transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
        }
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        movementComponent = this.GetComponent<PlayerMovementComponent>();
        
        if (!movementComponent)
            Debug.LogError(this.name + " is missing a MoveComponent!");
        if (!healthBarScript)
            Debug.LogError(this.name + " is missing a HealthBarScript!");
    }

    private void updateAnimations()
    {
    }

    public void onCreate()
    {
        tamedMounts = GameObject.FindGameObjectsWithTag("TamedMount"); //Populate tamed mounts
    }


    public void DisMount(float dismountDistance = 10f)
    {
        movementComponent.activeMount = null;
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
        return movementComponent.isRiding;
    }

    public void setIsRiding(bool passedVal)
    {
        movementComponent.isRiding = passedVal;
    }
    public void setActiveMount(Mount mount)
    {
        movementComponent.activeMount = mount;
        movementComponent.isRiding = true;
    }

    public void moveToMountedPosition()
    {
        var activeMount = movementComponent.activeMount;
        
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
        movementComponent.characterController.Move(impact * Time.deltaTime);
    }

    //Update floating healthbar in world space.
    public void updateHealthBar()
    {
        healthBarScript.setHealth(currentHealth, maxHealth);
    }

    //Kill/death command, despawn and drop loot.
    public void feint()
    {
        Debug.Log("oof, you died");
        //Destroy(gameObject);
    }
}
