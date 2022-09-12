using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //Using to alert whoever cares that inventory system has changed
using System.Linq;

[System.Serializable]
public class InventorySystem 
{
    //Hold amount of inv slots we want.
    [SerializeField] private List<InventorySlot> inventorySlots;

    //Public getters
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChange; //Event that fires when we add an item to our inventory       

    //Constructor
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amount)
    {
        inventorySlots[0] = new InventorySlot(itemToAdd, amount);
        return true;
    }
}
