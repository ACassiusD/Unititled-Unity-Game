using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosionAOE : MonoBehaviour //Can implement a AEO interface at one point, try not to use inheritance.
{
    public Collider[] attackHitboxes;
    public bool isActiveDamage = false;
    public float damageCooldownTimer = 0.0f;
    //How often the player can be hit by the AOE Effect
    public float damageCooldown = 0.5f;
    public float despawnTimer = 0.5f;
    public int damage = 5;
    public bool debug = false;
    public GameObject attackImpactEffect;
    public float useCooldown = 8.0f;
    public float useCooldownTimer = 0f;

    private void Start()
    {
        damageCooldownTimer = 0;
    }

    //remove logic from Update for performance reasons
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
            var script = c.GetComponent<PlayerEntity>();
            int[] dmgValues;
            hitCount++;
            script.receiveDamage(damage, 3000);
            debugMsg += ("|Hit " + c.name);
        }
        if (debug)
        {
            Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
        }
        damageCooldownTimer = damageCooldown;
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
}
