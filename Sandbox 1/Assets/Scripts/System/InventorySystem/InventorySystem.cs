using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events; //Using to alert whoever cares that inventory system has changed

[System.Serializable]
public class InventorySystem
{
    //Hold amount of inv slots we want.
    [SerializeField] private List<InventorySlot> inventorySlots;

    //Public getters
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChange; //Event that fires when we add an item to our inventory       

    //Constructor that sets the amount of slots
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
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) //Check wheather item exists in inventory
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChange?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot)) //Gets first avaliable slot.
        {
            if (freeSlot.EnoughRoomLeftInStack(amount))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amount);
                OnInventorySlotChange?.Invoke(freeSlot);
                return true;
            }
            //Add implementation to only take what can fill the stack and check for another free slot to put the remainder in.
        }
        return false;
    }

    //Checks if item is in the inventory, an passes back approriate slot
    // Do any of our slots have the item to add in them?
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(slot => slot.ItemData == itemToAdd).ToList();
        return invSlot == null ? false : true;
    }

    //Get the first free slot.
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
