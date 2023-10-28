using System.Collections;
using UnityEngine;

public class PlayerCombatComponent : CombatComponent
{
    private Transform projectileFiringOrigin;
    public GameObject projectile;
    private PlayerControls playerControls;
    private Transform cam;
    public int power = 30;
    public float forwardVel = 50f;
    public float upwardVel = 50f;
    public float mass = 5f;
    public bool useGravity = false;
    public int rayRange = 2000;

    //Melee attack stuff will go in weapon class eventually.
    public MeleeAttack meleeAttackPrefab; 
    private MeleeAttack activeMeleeAttackInstance;

    //An event to let the movement controller know that it should be put in stun.
    public event System.Action<float> OnStunned = delegate { };

    private void Awake()
    {
        projectileFiringOrigin = transform.Find("Projectile_Origin");

        if (projectileFiringOrigin == null)
        {
            Debug.LogError("ProjectileOrigin not found on " + gameObject.name);
            return;
        }

        // Assuming that the PlayerManager and the PlayerEntity are accessible globally
        cam = PlayerManager.Instance.getPlayerScript().playerMovementComponent.cam.transform;

        playerControls = new PlayerControls();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageableEntity = other.GetComponent<IDamageable>();
        if (damageableEntity != null)
        {
            float someDamageAmount = 10f;  // Replace with your own logic to determine the damage amount
            damageableEntity.ReceiveDamage(someDamageAmount);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        // Check for MouseButton1 press
        bool mouseButton1Pressed = playerControls.Player.MouseButton1.WasPerformedThisFrame();
        // Check for MouseButton2 being held down
        bool mouseButton2Held = playerControls.Player.MouseButton2.IsPressed();

        // If only MouseButton1 is pressed (Melee Attack)
        if (mouseButton1Pressed && !mouseButton2Held)
        {
            StartCoroutine(HandleMeleeAttack());
        }

        // If both MouseButton2 and MouseButton1 are pressed (Fire Projectile)
        if (mouseButton1Pressed && mouseButton2Held)
        {
            FireProjectile();
        }
    }

    public override float ReceiveDamage(float damageAmount, int? knockBackForce, Vector3? direction = null)
    {
        float newCurrentHealthValue = base.ReceiveDamage(damageAmount, knockBackForce, direction);

        //Raise OnStunned Event here.
        OnStunned(1.5f);

        return newCurrentHealthValue;
    }


    private IEnumerator HandleMeleeAttack()
    {
        // Instantiate and position the MeleeAttack hitbox
        activeMeleeAttackInstance = Instantiate(meleeAttackPrefab, transform.position, transform.rotation);
        activeMeleeAttackInstance.transform.parent = transform;  // Make the player the parent of the hitbox

        // Enable the hitbox (and MeshRenderer for debugging)
        activeMeleeAttackInstance.attackHitbox.EnableHitbox();

        // Wait for some time (e.g., duration of the melee attack)
        yield return new WaitForSeconds(0.1f);  // Adjust this duration based on your actual animation duration

        // Disable the hitbox (and MeshRenderer for debugging)
        activeMeleeAttackInstance.attackHitbox.DisableHitbox();

        // Destroy the hitbox instance if you want to clean up
        Destroy(activeMeleeAttackInstance.gameObject);
    }


    public void FireProjectile()
    {
        // Rotate the character to face firing direction
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        // Instantiate and position new arrow
        GameObject newArrow = Instantiate(projectile);
        newArrow.transform.position = projectileFiringOrigin.transform.position;
        Rigidbody rb = newArrow.GetComponent<Rigidbody>();

        // Trace ray for aim target position and rotate accordingly
        RaycastHit hitinfo;
        int layerMask1 = 1 << 12;
        int layerMask2 = 1 << 11;
        int layerMask3 = 1 << 10;
        var finalmask = layerMask1 | layerMask2 | layerMask3;
        finalmask = ~finalmask;

        if (Physics.Raycast(cam.position, cam.transform.forward, out hitinfo, rayRange, finalmask))
        {
            newArrow.transform.LookAt(hitinfo.point);
        }
        else
        {
            newArrow.transform.rotation = Quaternion.Euler(cam.localEulerAngles.x, transform.eulerAngles.y, 0);
        }

        // Add Force
        rb.AddForce(newArrow.transform.forward * forwardVel);
        rb.AddForce(newArrow.transform.up * upwardVel);
        rb.useGravity = useGravity;
        rb.mass = mass;
    }

    /// <summary>
    /// What happens when this entity dies.
    /// </summary>
    public override void Feint()
    {
        Debug.Log("Player Character Died");
        if (statsComponent.IsDead())
        {
            // Destroy(gameObject);
        }
    }
}
