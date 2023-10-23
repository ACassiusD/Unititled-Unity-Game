using UnityEngine;

//A projectile attack represents an attack that will be instantiated apon a player firing it
//Projectiles fly through the air until it collides with something.
//If the projectiel collides with a damagable entity it will do damage to that entity.
public class Projectile : Attack
{
    public bool enableRotation = true;
    public bool destroyOnHit = true;
    public float despawnTimer = 4f;

    public void Awake()
    {
        knockbackForce = 100;
    }

    private void Update()
    {
        // Rotate the arrow based on its velocity
        if (enableRotation && rb.velocity.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        // Handle despawn timer
        UpdateDespawnTimer();

        // Additional projectile-specific behavior, if needed
        SpecificAttackBehaviour();
    }

    public override void SpecificAttackBehaviour()
    {
        // For now, it can be empty or you can add additional projectile-specific logic
    }

    // Override the HandleHit from the base Attack class
    protected override void HandleHit(Collider other)
    {
        // Ignore collisions with the player due to the firing origin emanating from the player's body
        if (other.CompareTag("Player"))
            return;

        // Check if the collided object implements the IDamagable interface
        IDamageable damageableEntity = other.GetComponent<IDamageable>();
        if (damageableEntity != null)
        {
            var knockbackDirection = rb.velocity.normalized;

            damageableEntity.ReceiveDamage(damage, knockbackForce, knockbackDirection);
            //Debug.Log($"{this.name} Hit {other.name}");

            if (destroyOnHit)
            {
                Destroy(this.gameObject);
            }
        }
        Destroy(this.gameObject);
    }


    // Decrease the despawn timer and destroy the arrow when time runs out
    private void UpdateDespawnTimer()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
