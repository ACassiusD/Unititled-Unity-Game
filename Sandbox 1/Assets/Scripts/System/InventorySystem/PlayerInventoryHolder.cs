using UnityEngine;
using UnityEngine.Events;

/**
 * Represents the player's inventory with 2 different inventories (hotbar and backpack)
 */
public class PlayerInventoryHolder : InventoryHolder
{
    PlayerControls playerControls;
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    //Getter
    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;

    protected override void Awake()
    {
        playerControls = new PlayerControls();
        base.Awake();
        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    void Update()
    {
        if (playerControls.Player.InventoryKey.WasPerformedThisFrame())
        {
            OnPlayerBackpackDisplayRequested?.Invoke(SecondaryInventorySystem);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
            return true;
        if (secondaryInventorySystem.AddToInventory(data, amount))
            return true;
        return false;
    }

}
