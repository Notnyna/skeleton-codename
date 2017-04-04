using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class itemDatabase : MonoBehaviour {

    public List<item> items = new List<item>();

    private void Start()
    {
        ItemDatabaseSetup();
    }
    private void ItemDatabaseSetup()
    {
        items.Add(new item("Sword", "Sword for warriors", "sword_item", 0));
        items.Add(new item("Mana Potion", "Increases your mana", "manapotion_item", 1));
        items.Add(new item("Wan", "Wan for magicians", "wan_item", 2));
    }

    public item GetItemByID(int id)
    {
        foreach(item itm in items)
        {
           if(itm.itemID.Equals(id))
            {
                //Debug.Log(itm.itemname + " is returned GetItemByID");
                return itm;
            }
        }


        Debug.LogError("Can't find item");
        return null;
    }

    public string getDescByID(int id) {
        foreach(item itm in items) {
            if(itm.itemID == id) {
                return itm.itemDescription;
            }
        }

        Debug.LogError("There is no description");
        return null;
    }

    public item GetItemBySpriteName(string spriteName) {
        foreach(item itm in items) {

            if(itm.itemSpriteName.Equals(spriteName)) {
                //Debug.Log(itm.itemname + " is returned GetItemBySpriteName");
                return itm;
            }
        }

        Debug.LogError("Can't find item");
        return null;
    }

    public void ListItems() {
        foreach(item itm in items) {
            Debug.Log(itm.itemID+" "+itm.itemname);
        }
    }
}
