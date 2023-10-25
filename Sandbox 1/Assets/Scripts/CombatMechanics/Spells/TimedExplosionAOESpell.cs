using UnityEngine;

public class TimedExplosionAOESpell : Attack
{
    public float damageCooldownTimer = 0.0f;
    //How often the player can be hit by the AOE Effect
    public float damageCooldown = 0.5f;
    public float despawnTimer = 0.5f;
    public Collider[] attackHitboxes;
    public bool debug = false;
    public GameObject attackImpactEffect;
    public float useCooldown = 8.0f;
    public float useCooldownTimer = 0f;

    private new void Start()
    {
        //base.Start();  // Ensure we call the base Attack's Start method //We dont actually need this.
        //TODO: Spells and projectiles should be 2 different base classes.
        damageCooldownTimer = 0;
    }

    void Update()
    {
        UpdateTimers();
        DespawnCheck();
    }

    void ApplyAoeDamage()
    {
        string debugMsg = "";
        int hitCount = 0;
        var hitboxCollider = attackHitboxes[0];
        var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Player"));
        foreach (Collider c in cols)
        {
            ApplyDamageToTarget(c);
            hitCount++;
            debugMsg += ("|Hit " + c.name);
        }
        if (debug)
        {
            Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
        }
        damageCooldownTimer = damageCooldown;
    }

    void ApplyDamageToTarget(Collider target)
    {
        // Check if the collided object implements the IDamagable interface
        IDamageable damageableEntity = target.GetComponent<IDamageable>();
        if (damageableEntity != null)
        {
            var knockbackDirection = (target.transform.position - transform.position).normalized;
            damageableEntity.ReceiveDamage(damage, knockbackForce, knockbackDirection);
        }
    }

    void UpdateTimers()
    {
        damageCooldownTimer -= Time.deltaTime;
        despawnTimer -= Time.deltaTime;
    }

    void DespawnCheck()
    {
        if (despawnTimer <= 0)
        {
            var impact = Instantiate(attackImpactEffect);
            impact.transform.position = this.transform.position;
            Destroy(gameObject);
            ApplyAoeDamage();
        }
    }

    public override void SpecificAttackBehaviour()
    {
        // For now, it can be empty or you can add additional AOE-specific logic
    }
}
