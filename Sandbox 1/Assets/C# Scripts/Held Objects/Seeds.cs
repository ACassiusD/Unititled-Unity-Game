using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : CollectableItem
{
    int seedId = 0;
    int cropId = 0;
    string type = "seed";
    string name = "Pumkin Seed";


    public Seeds(string name, int qty, bool isStackable) : base(name, qty, isStackable)
    {
    }

    void returnTest()
    {
        Debug.Log("return test");
    }
}
