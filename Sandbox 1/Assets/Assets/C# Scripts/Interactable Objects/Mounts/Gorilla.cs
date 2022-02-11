using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorilla : Mount
{
    public float gorillaWalkSpeed = 4;
    public float gorillaRunSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 4;
        runSpeed = 15;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        runSpeed = gorillaRunSpeed;
        walkSpeed = gorillaWalkSpeed;
        base.Update();
    }
}
