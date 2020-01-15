using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour {

    CollectableItem[] itemCollection;
    //Text[] hotbarText;
    Image[] hotbarColor;
    int hotbarCount = 5;
    int inventorySize = 10;
    int selectedIndex = 0;
    public string heldObject = null;

    void Start () {
        Debug.Log("Initialized Inventory Manager");
        itemCollection = new CollectableItem[inventorySize];
        //initalizeHotbar();
        createFakeInventory();
        //updateHotbar();
       // highlightSelectedIndex();
    }

    private void Update()
    {
        //CheckHotbarInput();
    }

    private void createFakeInventory()
    {
        //Initalize temp list of items
        itemCollection[0] = new CollectableItem("Shovel", 1);
        itemCollection[1] = new CollectableItem("Sword", 1);
        itemCollection[1] = new CollectableItem("NewSeeds", 1);
        itemCollection[3] = new CollectableItem("Seeds", 1);
        itemCollection[4] = new CollectableItem("Watering Pail", 1);
    }

    //Create a connection to the hotbar UI elements
    private void initalizeHotbar()
    {
        var hotbarText = new TextField[hotbarCount];
        hotbarColor = new Image[hotbarCount];

        for (int i = 0; i < hotbarCount; i++)
        {
            GameObject hotbarElement = GameObject.FindWithTag("hotbar_" + (i + 1));
            hotbarText[i] = hotbarElement.GetComponentInChildren<TextField>();
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
               // hotbarText[i].text = currentObject.itemName;
                Debug.Log(currentObject.itemName);
            }
        }
    }

    void highlightSelectedIndex()
    {
        for (int i = 0; i < hotbarColor.Length; i++)
        {
            hotbarColor[i].tintColor = Color.white;
        }
        hotbarColor[selectedIndex].tintColor = Color.red;
    }

    //This can be refactored
    void CheckHotbarInput()
    {
        //Get new index
        
        int beforeUpdate = selectedIndex;
        float scrollBarInput = Input.GetAxis("Mouse ScrollWheel");

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

        //Zoom In
        if (scrollBarInput > 0f)
        {
            selectedIndex--;
        }
        //Zoom out
        else if (scrollBarInput < 0f) // backwards
        {
            selectedIndex++;
        }

        if (selectedIndex > hotbarCount -1) 
        {
            selectedIndex = 0;
        }
        if (selectedIndex < 0)
        {
            selectedIndex = hotbarCount - 1;
        }

        //Highlight index
        if (selectedIndex != beforeUpdate)
        {
            highlightSelectedIndex();
        }

        heldObject = itemCollection[selectedIndex].itemName;
    }
}
