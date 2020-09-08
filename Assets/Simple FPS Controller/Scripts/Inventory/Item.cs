using System;

[Serializable]
public class Item
{
    // The index of itemData
    public int itemDataIndex;

    // Coordinates in the Inventory grid
    public int slotX, slotY;

    public Item(int slotX_ = 0, int slotY_ = 0, int itemDataIndex_ = 0)
    {
        slotX = slotX_;
        slotY = slotY_;
        itemDataIndex = itemDataIndex_;
    }
}