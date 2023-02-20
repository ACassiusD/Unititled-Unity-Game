using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
//Class given to entities that will have an inventory
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;

    //Unity action is similar to a unity event that can be invoked, it takes no paramaters and returns no values.
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}
