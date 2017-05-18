using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour {


    public List<GameObject> slots = new List<GameObject>();

    public GameObject slotPrefab;
    public GameObject slotParent;
    public GameObject inventoryObject;
    public GameObject itemPrefab;

    public int slotAmount = 3;

    public KeyCode inventoryKey = KeyCode.Tab;
    public bool isShown = false;

    private itemDatabase itemDatabase;

    private toolTip tooltip;

    public static int invItemCount = 0;

	void Start () {
        createSlots(slotAmount);

        //Find ItemDatabase
        itemDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<itemDatabase>();

        // Find tooltip
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<toolTip>();

        AddItem(0); //Add sword to inventory
        AddItem(1); //Add mana to inventory
        AddItem(2); //Add wan to inventory
    }
	

	void Update () {
        if(Input.GetKeyDown(KeyCode.P)) {
            itemDatabase.ListItems();
        }
        if(Input.GetKeyDown(inventoryKey)) {
            isShown = !isShown;
            if(isShown) {
                tooltip.ToggleToolTip(false);
            }
        }

        if (isShown)
        {
            inventoryObject.SetActive(true);
        }
        else if (!isShown)
        {
            inventoryObject.SetActive(false);
        }
	}

    private void createSlots(int slotAmount)
    {
        for(int i=0; i < slotAmount; i++)
        {
            slots.Add(Instantiate(slotPrefab));
            slots[i].transform.SetParent(slotParent.transform);
        }
    }

    public void AddItem(int id)
    {
        item itemAdd = itemDatabase.GetItemByID(id);
        
        for(int i = 0; i < slots.Count; i++)
        {
            if(slots[i].transform.childCount <= 0)
            {
                itemPrefab.GetComponent<Image>().sprite = itemAdd.itemSprite; // set sprit of it
                GameObject itemInstance = Instantiate(itemPrefab); // creates item object
                itemInstance.transform.SetParent(slots[i].transform); // set Item parent
                itemInstance.transform.localPosition = Vector2.zero; // set item local position (0, 0)
                invItemCount++;
                //Debug.Log(invItemCount);
                break; // break out of loop when we add item
            }
        }
        
    }

}
