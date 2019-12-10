using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem {

    private bool isStackable;
    private int qty;
    public string itemName;

    //Constructor
    public CollectableItem(string name, int qty)
    {
        this.itemName = name;
        this.qty = qty;
        //this.isStackable = isStackable;
    }
    
    public void ChangeName()
    {
        itemName = "newName";
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
