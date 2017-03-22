using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class item {

    public string itemname;
    public string itemDescription;
    public string itemSpriteName;
    public Sprite itemSprite;
    public int itemID;

    public item(string name, string description, string spriteName,int id)
    {
        itemname = name;
        itemDescription = description;
        itemSpriteName = spriteName;
        itemSprite = Resources.Load<Sprite>("TestItems/" + spriteName);

        itemID = id;
    }

    public item()
    {

    }
}
