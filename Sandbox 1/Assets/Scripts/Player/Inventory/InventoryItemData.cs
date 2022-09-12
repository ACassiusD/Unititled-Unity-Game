using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Menu")]
public class InventoryItemData : ScriptableObject
{
    public string displayName;
    [TextArea(4,4)]
    public string description;
    public Sprite icon;
    public int maxStackSize;

}
