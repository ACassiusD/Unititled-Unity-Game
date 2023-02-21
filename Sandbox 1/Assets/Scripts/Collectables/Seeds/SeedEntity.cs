using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeds : CollectableItem
{
    int id = 0;
    string name = "Pumkin Seed";
    int cropId = 0;
    string stages;



    public Seeds(string name, int qty, bool isStackable) : base(name, qty, isStackable)
    {
    }

    void returnTest()
    {
        Debug.Log("return test");
    }
}
