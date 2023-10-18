using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//A dynamic inventory is one that can change its size or other dynamic properties. Unlike the Player UI inventory which stays the same.
//Can be used to for things like chests.
public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;

    //Subscribes to all the inventory holders static OnDynamicInventoryDisplayRequested event. 
    //Invokea the RefreshDynamicInventory method when the event is triggered. Passing the InventorySystem indirectly.
    //Example onDynamicInventoryDisplayRequested.Invoke(inventorySystem);
    protected override void Start()
    {
        base.Start();
    }

    //Refresh own inventory.
    public void RefreshDynamicInventory(InventorySystem invToDisplay)
    {
        ClearSlots();
        inventorySystem = invToDisplay;

        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChange += UpdateSlot;

        AssignSlots(invToDisplay);
    }

    //Assign the slots to the dynamic inventory.
    public override void AssignSlots(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (slotDictionary != null) slotDictionary.Clear();
    }

    //When we close panel with escape key. Unsubscibe to event.
    private void OnDisable()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChange -= UpdateSlot;
    }
}
