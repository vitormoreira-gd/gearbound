using System;

[Serializable]
public class Item
{
    public string name;
    public string description;
    public ItemType type;

    public Item(
        string name,
        string description,
        ItemType type)
    {
        this.name = name;
        this.description = description;
        this.type = type;
    }
}
