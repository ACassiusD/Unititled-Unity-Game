﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorilla : Mount
{
    // Start is called before the first frame update
    void Start()
    {
        walkSpeed = 33;
        runSpeed = 99;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}