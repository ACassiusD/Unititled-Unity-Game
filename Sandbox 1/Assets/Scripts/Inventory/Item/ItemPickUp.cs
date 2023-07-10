using SaveLoadSystem;
using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
//Give this class to a game object and set the inventory data to allow it to be picked up and stored in an inventory.
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1.5f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;


    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);


        int LayerIgnoreRaycast = LayerMask.NameToLayer("Collectable");
        gameObject.layer = LayerIgnoreRaycast;

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if(SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if what we colided with has an inventory component. 
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;

        //If it does. Add it to their inventory.
        if (inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
            return;
        }

        //Allow players to pick up collectables when riding a mount.
        if (other.tag == "TamedMount")
        {
            Mount mountScript = other.GetComponent<Mount>();
            BetaCharacter owner = mountScript.getOwner();

            var parentInventory = owner.inventory;
       
            if (!parentInventory) return;

            if (parentInventory.PrimaryInventorySystem.AddToInventory(ItemData, 1))
            {
                Destroy(this.gameObject);
            }
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public quaternion Rotation;

    public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, quaternion _roataion)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _roataion;
    }
}