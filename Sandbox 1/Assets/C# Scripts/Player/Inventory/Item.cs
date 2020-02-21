//Script applied to every single item type object in our game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MANA, HEALTH };

public class Item : MonoBehaviour
{
    public ItemType type;
   
    //Neutral and highlighted sprites for use in the inventory
    public Sprite spriteNeutral; 
    public Sprite spriteHightlighted;

    public int maxSize; //How many times an item can stack

    public void Use()
    {
        switch (type)
        {
            case ItemType.MANA:
                Debug.Log("Used mana potion");
                break;
            case ItemType.HEALTH:
                Debug.Log("Used health potion");
                break;
        }
    }


}
