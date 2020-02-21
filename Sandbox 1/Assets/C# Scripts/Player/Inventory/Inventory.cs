using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//An inventory, holds a list of slots that is dynamically sized based on the number of rows and columns given
public class Inventory : MonoBehaviour
{
    private RectTransform inventoryRect;
    private float inventoryWidth, inventoryHeight;
    public int slots;
    public int rows;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    private static Slot from, to;
    public GameObject slotPrefab; //Prefab for a slot
    private List<GameObject> allSlots; //Holds all inventory slots
    public GameObject iconPrefab;
    private static GameObject hoverObject;
    private static int emptySlots;
    public Canvas canvas;
    private float hoverYOffset;
    public EventSystem eventsystem;

    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLayout();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(!eventsystem.IsPointerOverGameObject(-1) && from != null)
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;

            }
        }
        if(hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);

        }
    }

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        EmptySlots = slots;
       
        //Dynamically calculate hight and width of the inventory
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        //Get the inventory rect transform
        inventoryRect = GetComponent<RectTransform>();

        //Apply the calculations to set the dynamic height and width
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        //Calculate the x,y position for the slots
        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                //Create the slot, and set the transform
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                //Set position
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
                //Set size
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                allSlots.Add(newSlot);
            }

        }
    }

    //Add item to the inventory
    public bool AddItem(Item item)
    {
        if(item.maxSize == 1)
        {
            PlaceInEmpty(item);
            return true;

        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmpSlot = slot.GetComponent<Slot>();

                if (!tmpSlot.isEmpty)
                {
                    //Found slot with same item, that has avaliable stacking storage
                    if(tmpSlot.CurrentItem.type == item.type && tmpSlot.IsAvaliable)
                    {
                        tmpSlot.AddItem(item);
                        return true;
                    }
                }
            }

            if(EmptySlots > 0)
            {
                PlaceInEmpty(item);
            }
        }
        return false;
    }

    //Runs though collection of slots for an empty slot, and places an item inside if found.
    private bool PlaceInEmpty(Item item)
    {
        if(EmptySlots > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot slotScript = slot.GetComponent<Slot>();

                if (slotScript.isEmpty)
                {
                    slotScript.AddItem(item);
                    EmptySlots--;
                    return true;
                }
            }
        }

        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if(from == null) //First item we have clicked
        {
            if (!clicked.GetComponent<Slot>().isEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.y);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.x);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }

        if(to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if(tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }
}
