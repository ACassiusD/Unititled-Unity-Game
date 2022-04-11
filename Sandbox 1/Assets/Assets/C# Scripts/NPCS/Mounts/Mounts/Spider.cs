
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Mount
{
    public float spiderAnimationWalkSpeed = 0.85f;
    public float spiderAnimaitonRunSpeed = 2;
    public Collider[] attackHitboxes;

    void Start()
    {
        runAnimaitonSpeed = spiderAnimaitonRunSpeed;
        walkAnimationSpeed = spiderAnimationWalkSpeed;
        base.Start();
    }

    public override void attack()
    {
        string debugMsg = "";
        int hitCount = 0;
        var hitboxCollider = attackHitboxes[0];
        var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Interactive"));
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                return;
            }
            if (c.tag == "Enemy")
            {
                int[] dmgValues;
                var attackValues = new Dictionary<string, int>();
                attackValues.Add("damage", 10);
                attackValues.Add("knockback", 3000);
                hitCount++;
                c.SendMessageUpwards("receiveDamage", attackValues);
                debugMsg += ("|Hit " + c.name);
            }
        }
        Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
    }
}
