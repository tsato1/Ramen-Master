using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    private Stack<Item> items;

    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public Item CurrentItem
    {
        get { return items.Peek(); }
    }

    public Sprite slotEmpty;

    public Sprite slotHighlighted;



	void Awake () 
    {
        items = new Stack<Item>();
	}

    public void AddItem(Item item)
    {
        items.Push(item);

        ChangeSprites(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);

        ChangeSprites(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    void ChangeSprites(Sprite neutral, Sprite highlighted)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();

        st.highlightedSprite = highlighted;
        st.pressedSprite = neutral;
    }

    public void ClearSlot()
    {
        items.Clear();

        ChangeSprites(slotEmpty, slotHighlighted);
    }
}
