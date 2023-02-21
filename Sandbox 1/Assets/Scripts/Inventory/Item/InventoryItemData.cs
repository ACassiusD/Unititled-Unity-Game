using UnityEngine;

/**
 * A scriptable object, that defines what an item is in our game.
 * It could be inherited from to have branched version of items, for example, potions and equipment.
 */
[CreateAssetMenu(menuName = "Inventory System/Inventory Menu")]
public class InventoryItemData : ScriptableObject
{
    public string displayName;
    [TextArea(4,4)]
    public string description;
    public Sprite icon;
    public int maxStackSize;
    public int goldValue;
}
