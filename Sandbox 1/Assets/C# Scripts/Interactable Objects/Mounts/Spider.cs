using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Mount
{
    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 50;
        runSpeed = 100;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
