using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Mount
{
    public int spiderWalkSpeed = 50;
    public int spiderRunSpeed = 100;
    public float spiderAnimationWalkSpeed = 0.85f;
    public float spiderAnimaitonRunSpeed = 2;
    // Start is called before the first frame update
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
}
