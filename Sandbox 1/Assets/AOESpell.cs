using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpell : MonoBehaviour
{
    public Collider[] attackHitboxes;
    public bool isActiveDamage = false;


    //remove logic from Update for performance reasons
    void Update()
    {
        if (isActiveDamage)
        {
            string debugMsg = "";
            int hitCount = 0;
            var hitboxCollider = attackHitboxes[0];
            var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Player"));
            foreach (Collider c in cols)
            {
                int[] dmgValues;
                var attackValues = new Dictionary<string, int>();
                attackValues.Add("damage", 2);
                attackValues.Add("knockback", 3000);
                hitCount++;
                c.SendMessageUpwards("receiveDamage", attackValues);
                debugMsg += ("|Hit " + c.name);
            }
            Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
           // isActiveDamage = false;
        }
        
    }
}
