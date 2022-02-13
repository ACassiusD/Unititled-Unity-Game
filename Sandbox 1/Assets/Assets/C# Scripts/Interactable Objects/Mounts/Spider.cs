using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Mount
{
    public int spiderWalkSpeed = 50;
    public int spiderRunSpeed = 100;
    public float spiderAnimationWalkSpeed = 0.85f;
    public float spiderAnimaitonRunSpeed = 2;
    public Collider[] attackHitboxes;

    void Start()
    {
        runAnimaitonSpeed = spiderAnimaitonRunSpeed;
        walkAnimationSpeed = spiderAnimationWalkSpeed;
        walkSpeed = spiderWalkSpeed;
        runSpeed = spiderRunSpeed;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        runAnimaitonSpeed = spiderAnimaitonRunSpeed;
        walkAnimationSpeed = spiderAnimationWalkSpeed;
        walkSpeed = spiderWalkSpeed;
        runSpeed = spiderRunSpeed;
        base.Update();
    }

    public override void attack()
    {
        string debugMsg = "";
        int hitCount = 0;
        var hitboxCollider = attackHitboxes[0];
        var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Interactive"));
        foreach(Collider c in cols)
        {
            if(c.tag == "Player"){
                return;
            }

            hitCount++;
            c.SendMessageUpwards("receiveDamage", 10);
            debugMsg += ("|Hit " + c.name);
        }
        Debug.Log(this.name + " Attacks - Hit (" + hitCount + ") " + debugMsg);
    }
}
