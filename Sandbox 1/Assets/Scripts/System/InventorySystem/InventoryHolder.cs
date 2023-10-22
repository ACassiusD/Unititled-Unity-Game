using UnityEngine;
using UnityEngine.Events;

/**
 * Class given to entities that will have an inventory,
 * Can be overridden to give more functionallity, i.e. PlayerInventoryHolder 
 */
[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem primaryInventorySystem;

    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    //Unity action is similar to a unity event that can be invoked, it takes no paramaters and returns no values.
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        primaryInventorySystem = new InventorySystem(inventorySize);
    }
}
