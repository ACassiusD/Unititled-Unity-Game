using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//test
public class Slot : MonoBehaviour//, IPointerClickHandler
{
    private Stack<Item> items; //Stack is a last in first out collection of items, use push(), pop() and peek()
    //public Text stackText; 
    public Sprite slotEmpty; //Sprite when slot is empty
    public Sprite slotHightlight; //Sprite when slot is highlighted

    //public bool isEmpty
    //{
    //    get { return Items.Count == 0;  }
    //}

    ////Returns current item at the top of the stack but does not remove it
    //public Item CurrentItem
    //{
    //   get { return Items.Peek(); }
    //}

    ////Compare how many items are held in this stack vs the items stack size limit.
    //public bool IsAvaliable
    //{
    //    get { return CurrentItem.maxSize > Items.Count; }
    //}

    //public Stack<Item> Items
    //{
    //    get
    //    {
    //        return items;
    //    }

    //    set
    //    {
    //        items = value;
    //    }
    //}

    //private void Start()
    //{
    //    Items = new Stack<Item>();
    //    RectTransform slotRect = this.GetComponent<RectTransform>();
    //    RectTransform txtRect = stackText.GetComponent<RectTransform>();

    //    //Calculate and set the text scale
    //    int textScale = (int)(slotRect.sizeDelta.x * 0.60);

    //    stackText.resizeTextMaxSize = textScale;
    //    stackText.resizeTextMinSize = textScale;

    //    txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
    //    txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

    //    Vector3 textPosition = new Vector3();
    //    float textWidth = txtRect.rect.width;

    //    textPosition.Set(textWidth / 2, textWidth / 2 * -1, 0);
    //    txtRect.transform.localPosition = textPosition;
    //}

    //public void AddItem(Item item)
    //{
    //    //Add item to the stack
    //    Items.Push(item);

    //    //Set the stack text if it is approriate
    //    if(Items.Count > 1)
    //    {
    //        stackText.text = Items.Count.ToString();
    //    }

    //    //Change the sprites for this slot
    //    ChangeSprite(item.spriteNeutral, item.spriteHightlighted);
    //}

    ////Change the item stack, update the item count and the sprites
    //public void AddItems(Stack<Item> items)
    //{
    //    this.Items = new Stack<Item>(items);
    //    stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
    //    ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHightlighted);
    //}

    //private void ChangeSprite(Sprite neutral, Sprite hightlight)
    //{
    //    //Change the default sprite
    //    GetComponent<Image>().sprite = neutral;

    //    //Change the sprite states of the button
    //    SpriteState spriteState = new SpriteState();
    //    spriteState.highlightedSprite = hightlight;
    //    spriteState.pressedSprite = neutral;
    //    GetComponent<Button>().spriteState = spriteState;
    //}

    //private void UseItem()
    //{
    //    if(!isEmpty){
    //        Items.Pop().Use();

    //        stackText.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

    //        if (isEmpty)
    //        {
    //            ChangeSprite(slotEmpty, slotHightlight);
    //            Inventory.EmptySlots++;
    //        }
    //    }
    //}

    //public void ClearSlot()
    //{
    //    items.Clear();
    //    ChangeSprite(slotEmpty, slotHightlight);
    //    stackText.text = string.Empty;
    //}

    ////Gets called when clicked
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.CanvasGroup.alpha > 0)
    //    {
    //        UseItem();
    //    }
    //}
}
