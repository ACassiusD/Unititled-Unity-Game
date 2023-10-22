using UnityEngine;

// Base class for all types of attacks
public abstract class Attack : MonoBehaviour, IAttackBehaviour
{
    // Reference to the Hitbox script attached to the attack's hitbox
    public Hitbox attackHitbox;
    public float damage = 50;

    //Only used if the attack utilizes knockback.
    protected Rigidbody rb;
    public int knockbackForce = 100;
    protected Vector3 knockbackDirection;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Subscribe to the hitbox's On Hit event
        attackHitbox.OnHitEvent.AddListener(HandleHit);
    }

    //Common hit handling logic for all attacks
    protected virtual void HandleHit(Collider other)
    {
        // Ignore collisions with the player
        if (other.CompareTag("Player"))
            return;

        // Check if the collided object implements the IDamagable interface
        IDamageable damageableEntity = other.GetComponent<IDamageable>();
        if (damageableEntity != null)
        {
            // Calculate knockback direction if the current attack object has a Rigidbody
            knockbackDirection = rb != null ? rb.velocity.normalized : transform.forward;

            // Call ReceiveDamage on the damageable entity
            damageableEntity.ReceiveDamage(damage, knockbackForce, knockbackDirection);
            Debug.Log($"{this.name} Hit {other.name}");
        }
    }

    // Abstract method to be implemented by derived classes for specific behaviors
    public abstract void SpecificAttackBehaviour();
}