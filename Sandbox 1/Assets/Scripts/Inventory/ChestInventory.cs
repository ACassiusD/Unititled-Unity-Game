using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

//Class represents a Chest that is interactable and manages its own inventory including the saving and loading of that inventory via subscribing to the saveload systems onload unity action.
[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    //Inherited by IInteractable.
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }


    //Laod inventory 
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory; //Subscribe to onLoad event.
    }

    private void Start()
    {
        var chestSaveData = new ChestSaveData(primaryInventorySystem, transform.position, transform.rotation);

        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    private void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            this.primaryInventorySystem = chestData.inventorySystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        //If the unity event OnDynamicInventoryDisplayRequested  is not null, invoke it.
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {

    }
}

//Serializable save data structure for the chests inventory.
[System.Serializable]
public struct ChestSaveData
{
    public InventorySystem inventorySystem;
    public Vector3 position;
    public Quaternion rotation;

    //Constructor
    public ChestSaveData(InventorySystem _inventorySystem, Vector3 _position, Quaternion _rotation)
    {
        inventorySystem = _inventorySystem;
        position = _position;
        rotation = _rotation;
    }


}
