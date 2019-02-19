using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;

    public Item(int id, string itemName, string description, Sprite icon)
    {
        this.id = id;                       //Holt sich die ID des Items
        this.itemName = itemName;           //Holt sich den Item Namen des Items
        this.description = description;     //Holt sich die Beschreibung des Items
        this.icon = icon;                   //Holt sich das Icon des Items
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.itemName = item.itemName;
        this.description = item.description;
        this.icon = item.icon;
    }
}
