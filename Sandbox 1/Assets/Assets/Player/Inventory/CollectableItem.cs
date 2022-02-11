using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem {

    protected string type = "null";
    protected bool isStackable = false;
    protected int qty = 0;
    protected string name = "collectable_item";

    //Constructor
    public CollectableItem(string name, int qty,  bool isStackable)
    {
        this.name = name;
        this.qty = qty;
        this.isStackable = isStackable;
    }
    
    //GETTERS
    public string getName()
    {
        return name;
    }

    public int getQty()
    {
        return qty;
    }

    public bool getIsStackable()
    {
        return isStackable;
    }

    //SETTERS
    public void setName(string name)
    {
        this.name = name;
    }

    public void setQty(int qty)
    {
        this.qty = qty;
    }

    public void setIsStackable(bool isStackable)
    {
        this.isStackable = isStackable;
    }
}
