using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
//Give this class to a game object and set the inventory data to allow it to be picked up and stored in an inventory.
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1.5f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider;

    private void Awake()
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Collectable");
        gameObject.layer = LayerIgnoreRaycast;

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData, 1))
        {
            Destroy(this.gameObject);
            return;
        }

        if (other.tag == "TamedMount")
        {
            Mount mountScript = other.GetComponent<Mount>();
            BetaCharacter owner = mountScript.getOwner();

            var parentInventory = owner.inventory;
       
            if (!parentInventory) return;

            if (parentInventory.InventorySystem.AddToInventory(ItemData, 1))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
