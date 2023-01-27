using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slot class. Represents individual slot to hold items in the inventory. 
[System.Serializable] //See in inspector.
public class InventorySlot //Isnt a monobehavior, doesnt need to start or update, not in the game scene.

{
    [SerializeField] private InventoryItemData itemData; //Reference to the data
    [SerializeField] private int stackSize; //Current stack size - how many of the data do we have?

    //getters
    public InventoryItemData ItemData => itemData; 
    public int StackSize => stackSize;

    //Constructor to make a empty inventory slot
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }
    
    //Null constructor
    public InventorySlot()
    {
        ClearSlot();
    }

    //Assigns an item to the slot
    public void AssignItem(InventorySlot invSlot)
    {
        //If invenotryslot data already has the same item we are trying to add.
        if (itemData == invSlot.itemData) AddToStack(invSlot.stackSize);
        else //overwrite slot with inv slot we are passing in
        {
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    //Updates slot directly
    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        itemData = data;
        stackSize = amount;
    }

    //Clear Slot
    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.maxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }


    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (itemData == null || itemData != null && stackSize + amountToAdd <= itemData.maxStackSize) return true;
        else return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        if(stackSize <= 1) //Is there enought to split
        {
            splitStack = null;
            splitStack = null;
            return false;
        }

        int halfstack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfstack);

        splitStack = new InventorySlot(itemData, halfstack);

        return true;
    }

}
