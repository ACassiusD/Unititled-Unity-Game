using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerCombatComponent : CombatComponent
{
    public GameObject projectile;
    public int power = 30;
    private Transform projectileFiringOrigin;
    public float forwardVel = 50f;
    public float upwardVel = 50f;
    public float mass = 5f;
    public bool useGravity = false;
    public int rayRange = 2000;
    private PlayerControls playerControls;
    private Transform cam;

    //For Melee Attack
    public VisualEffect meleeAnimation;
    public Collider meleeHitbox;
    private bool isMeleeAttacking = false;

    private void Awake()
    {
        //Get Melee Hitbox. Throw an error if not found.
        meleeHitbox = transform.Find("Melee_Attack/Melee_Hitbox").GetComponent<Collider>();
        if (meleeHitbox == null)
            Debug.LogError("Melee_Hitbox not found on " + gameObject.name);

        // Assuming that the PlayerManager and the PlayerEntity are accessible globally
        cam = PlayerManager.Instance.getPlayerScript().playerMovementComponent.cam.transform;

        // Get the projectileOrigin transform from the entity
        projectileFiringOrigin = transform.Find("Projectile_Origin");
        if (projectileFiringOrigin == null)
        {
            Debug.LogError("ProjectileOrigin transform not found on " + gameObject.name);
            return;
        }

        playerControls = new PlayerControls();
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
        bool capturedKeyPress = playerControls.Player.MouseButton1.WasPerformedThisFrame();
        if (capturedKeyPress)
        {
            FireProjectile();
        }

        // Merged MeleeAnimation logic
        if (playerControls.Player.MeleeKey.WasPerformedThisFrame())
        {
            meleeAnimation.Play();
            StartCoroutine(HandleMeleeAttack());
        }
    }

    private IEnumerator HandleMeleeAttack()
    {
        isMeleeAttacking = true;
        meleeHitbox.enabled = true;

        // Wait for some time (e.g., duration of the melee attack)
        yield return new WaitForSeconds(0.5f); // This duration needs to be adjusted based on your animation duration

        meleeHitbox.enabled = false;
        isMeleeAttacking = false;
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

    public float ReceiveDamage(float damageAmount, int? knockBackForce, Vector3? direction = null)
    {
        TakeDamage(damageAmount);
        // playerMovementComponent.stunTimer = playerMovementComponent.stunDuration;
        return statsComponent.currentHealth;
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
