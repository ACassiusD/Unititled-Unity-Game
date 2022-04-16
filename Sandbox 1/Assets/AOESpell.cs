using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpell : MonoBehaviour
{
    public Collider[] attackHitboxes;
    public bool isActiveDamage = false;
    public float timer = 0.0f;
    public float cooldown = 0.5f;
    public int damage = 5;
    public bool debug = false;

    private void Start()
    {
        timer = cooldown;
    }

    //remove logic from Update for performance reasons
    void Update()
    {
        timer -= Time.deltaTime;
        if (isActiveDamage && timer <= 0)
        {
            timer = cooldown;
            string debugMsg = "";
            int hitCount = 0;
            var hitboxCollider = attackHitboxes[0];
            var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Player"));
            foreach (Collider c in cols)
            {
                int[] dmgValues;
                var attackValues = new Dictionary<string, int>();
                attackValues.Add("damage", damage);
                attackValues.Add("knockback", 3000);
                hitCount++;
                c.SendMessageUpwards("receiveDamage", attackValues);
                debugMsg += ("|Hit " + c.name);
            }
            if (debug)
            {
                Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
            }
        }
    }
}
