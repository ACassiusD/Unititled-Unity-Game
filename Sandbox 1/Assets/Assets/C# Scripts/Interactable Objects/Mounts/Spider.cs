﻿using System.Collections;
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
        //TESTS
        var canvas = GameObject.Find("Canvas");
        RectTransform test2 = canvas.GetComponent<RectTransform>();
        var offset = 3.5f;
        Vector3 newPos = this.transform.position;
        newPos.y += offset;

        test2.localPosition = newPos;
        //TESTS



        runAnimaitonSpeed = spiderAnimaitonRunSpeed;
        walkAnimationSpeed = spiderAnimationWalkSpeed;
        walkSpeed = spiderWalkSpeed;
        runSpeed = spiderRunSpeed;
        base.Update();
    }

    public override void attack()
    {
        var hitboxCollider = attackHitboxes[0];
        var cols = Physics.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.extents, hitboxCollider.transform.rotation, LayerMask.GetMask("Interactive"));
        foreach(Collider c in cols)
        {
            Debug.Log(c.name + "hit");
        }
        Debug.Log(this.name + " Attacks ");
    }
}