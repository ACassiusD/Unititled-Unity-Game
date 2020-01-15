using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : CollectableItem
{

    public Seeds(string name, int qty) : base(name, qty)
    {
    }

    void returnTest()
    {
        Debug.Log("return test");
    }
}
