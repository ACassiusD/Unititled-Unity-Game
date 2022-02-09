using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Mount
{
    public int spiderWalkSpeed = 50;
    public int spiderRunSpeed = 100;
    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = spiderWalkSpeed;
        runSpeed = spiderRunSpeed;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        walkSpeed = spiderWalkSpeed;
        runSpeed = spiderRunSpeed;
        base.Update();
    }
}
