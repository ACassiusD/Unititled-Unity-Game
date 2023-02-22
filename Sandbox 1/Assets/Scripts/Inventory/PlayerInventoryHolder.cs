using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

    protected override void Awake()
    {
        playerControls = new PlayerControls();
        base.Awake();
        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    void Update()
    {
        if (playerControls.Player.BKey.WasPerformedThisFrame())
        {
            OnDynamicInventoryDisplayRequested?.Invoke(SecondaryInventorySystem);
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

}
