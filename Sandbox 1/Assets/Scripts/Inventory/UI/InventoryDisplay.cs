using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//An abstract UI representation of an inventory, could be a player inventory or a storage chest inventory etc.
public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;
    
    protected InventorySystem inventorySystem; //Inventory system we want to display

    //Create a dictionary of UI slot elements that will be displayed to represent the inventory
    //Match each ui slot representation with its backend class
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
                   
    //Public getters
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    public abstract void AssignSlot(InventorySystem invToDisplay); //Abstract classes must be overridden

    protected virtual void Start()
    {

    }

    //A backend slot is passed, we loop through the Inventories SlotDictionary until we find the slot we passed on, then update the ui slot that coorosponds to it.
    protected virtual void UpdateSlot(InventorySlot updatedSlot) //Virutal classes override is optional
    {
        foreach(var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) //backend slot
            {
                slot.Key.UpdateUISlot(updatedSlot); //ui slot
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        //Clicked slot has an item - mouse doesnt have an item - pick up that item

        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if(clickedUISlot.AssignedInventorySlot.ItemData != null & mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            //If the player is holding shift key? Split the stack.
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack( out InventorySlot halfStackSlot))// split stack
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }


        //clicked clot doesnt ahve an item - mouse does have an item - place mthe mouse item into the empty sot.
        if(clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        { 
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        //are both items the safe? if so combine them/
        //is the slot stack size + mosue stack size ? the slot max stack size? is so, take from mouse.
        //If different items, then swap the item.

        //If both slots have an item - decide what to do...
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            else if(isSameItem && 
                !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1) SwapSlots(clickedUISlot); // Stack is full so swap the items
                else //Slot is not at max, so take whats needed from the mouse inventory.
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            else if(!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        //Clone mouse data
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);

        //Clear mouse slot data
        mouseInventoryItem.ClearSlot();

        //Pass cliked slot
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
