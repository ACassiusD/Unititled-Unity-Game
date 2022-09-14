using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slot class. Represents individual slot to hold items in the inventory. 
[System.Serializable] //See in inspector.
public class InventorySlot //Isnt a monobehavior, doesnt need to start or update, not in the game scene.

{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    //Constructor
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

    public bool RoomlLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.maxStackSize - stackSize;
        return RoomLeftInStack(amountToAdd);
    }


    public bool RoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.maxStackSize) return true;
        else return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveToStack(int amount)
    {
        stackSize -= amount;
    }


}
