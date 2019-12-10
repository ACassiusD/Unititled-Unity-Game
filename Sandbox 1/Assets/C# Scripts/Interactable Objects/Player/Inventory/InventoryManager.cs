using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    CollectableItem[] itemCollection;
    Text[] hotbarText;
    Image[] hotbarColor;
    int hotbarCount = 5;
    int inventorySize = 10;
    CollectableItem pickedUpItem;
    int selectedIndex = 0;
    public string heldObject = null;

    void Start () {
        Debug.Log("Initialized Inventory Manager");
        itemCollection = new CollectableItem[inventorySize];
        initalizeHotbar();
        createFakeInventory();
        updateHotbar();
        highlightSelectedIndex();
    }

    private void Update()
    {
        CheckHotbarInput();
    }

    private void createFakeInventory()
    {
        //Initalize temp list of items
        itemCollection[0] = new CollectableItem("Shovel", 1);
        itemCollection[1] = new CollectableItem("Sword", 1);
        itemCollection[2] = new CollectableItem("Dounut", 1);
        itemCollection[3] = new CollectableItem("Seeds", 1);
        itemCollection[4] = new CollectableItem("Watering Pail", 1);
    }

    //Create a connection to the hotbar UI elements
    private void initalizeHotbar()
    {
        hotbarText = new Text[hotbarCount];
        hotbarColor = new Image[hotbarCount];

        for (int i = 0; i < hotbarCount; i++)
        {
            GameObject hotbarElement = GameObject.FindWithTag("hotbar_" + (i + 1));
            hotbarText[i] = hotbarElement.GetComponentInChildren<Text>();
            hotbarColor[i] = hotbarElement.GetComponentInChildren<Image>();
        }
    }

    //Update the hotbar UI button text
    private void updateHotbar()
    {
        CollectableItem currentObject = null; 

        for (int i = 0; i < itemCollection.Length; i++)
        {
            currentObject = itemCollection[i];
            if (currentObject is CollectableItem)
            {
                hotbarText[i].text = currentObject.itemName;
                Debug.Log(currentObject.itemName);
            }
        }
    }

    void highlightSelectedIndex()
    {
        for (int i = 0; i < hotbarColor.Length; i++)
        {
            hotbarColor[i].color = Color.white;
        }
        hotbarColor[selectedIndex].color = Color.red;
    }

    //This can be refactored
    void CheckHotbarInput()
    {
        int beforeUpdate = selectedIndex;

        //Try to call interact
        if (Input.GetKeyDown("1"))
        {
            selectedIndex = 0;
        }

        if (Input.GetKeyDown("2"))
        {
            selectedIndex = 1;
        }

        if (Input.GetKeyDown("3"))
        {
            selectedIndex = 2;
        }

        if (Input.GetKeyDown("4"))
        {
            selectedIndex = 3;
        }

        if (Input.GetKeyDown("5"))
        {
            selectedIndex = 4;
        }

        if(selectedIndex != beforeUpdate)
        {
            highlightSelectedIndex();
        }

        heldObject = itemCollection[selectedIndex].itemName;
    }
}
