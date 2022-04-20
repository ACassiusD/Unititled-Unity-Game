using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpell : MonoBehaviour
{
    public Collider[] attackHitboxes;
    public bool isActiveDamage = false;
    public float timer = 0.0f;
    public float cooldown = 0.5f;
    public float despawnTimer = 0.5f;
    public int damage = 5;
    public bool debug = false;
    public bool isStatic = false;

    private void Start()
    {
        timer = 0;
    }

    //remove logic from Update for performance reasons
    void Update()
    {
        UpdateTimers();
        DamageCheck();
        DespawnCheck();
    }

    void DamageCheck()
    {
        if (isActiveDamage && timer <= 0)
        {
            string debugMsg = "";
            int hitCount = 0;
            var hitboxCollider = attackHitboxes[0];
            var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Player"));
            foreach (Collider c in cols)
            {
                var script = c.GetComponent<BetaCharacter>();
                int[] dmgValues;
                hitCount++;
                script.receiveDamage(damage, 3000);
                debugMsg += ("|Hit " + c.name);
            }
            if (debug)
            {
                Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
            }
            timer = cooldown; 
        }
    }
    void UpdateTimers()
    {
        timer -= Time.deltaTime;
        despawnTimer -= Time.deltaTime;
    }

    void DespawnCheck(){
        if (isStatic) return;
        if (despawnTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
