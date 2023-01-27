using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Controls Inventory Slot UI Prefab.
public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot; //Public getter, no setter.
    public InventoryDisplay ParentDisplay{ get; private set; }

    private void Awake()
    {
        //Clear everything.
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";

        //Listen for onclick method, play OnUISlotClick()
        button = GetComponent<Button>();
        button.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    //Update UI slot by passing in a slot.
    public void UpdateUISlot(InventorySlot slot)
    {
        //Add slot data to this UI Slot representation
        if(slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;
            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else //Clear ui slot representation
        {
            ClearSlot();
        }
    }

    //Refresh a slot without passing in an inventory slot.
    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
