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
        items.Add(new item("Sword", "Sword for wariors", "sword_item", 0));
        items.Add(new item("Mana Potion", "Increases your mana", "manapotion_item", 1));
    }

    public item GetItemByID(int id)
    {
        foreach(item itm in items)
        {
           if(itm.itemID == id)
            {
                return itm;
            }
        }
        
        Debug.LogError("Can't find item");
        return null;
    }
}
