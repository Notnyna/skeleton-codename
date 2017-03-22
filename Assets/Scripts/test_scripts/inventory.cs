using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour {

    // Use this for initialization

    public List<GameObject> slots = new List<GameObject>();

    public GameObject slotPrefab;
    public GameObject slotParent;
    public GameObject inventoryObject;
    public GameObject itemPrefab;

    public int slotAmount = 5;

    public KeyCode inventoryKey = KeyCode.Tab;
    public bool isShown = true;

    private itemDatabase itemDatabase;

	void Start () {
        createSlots(slotAmount);

        //Find ItemDatabase
        itemDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<itemDatabase>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem(0);
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            isShown = !isShown;
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
                GameObject itemInstance = Instantiate(itemPrefab); // creates item object
                itemInstance.transform.SetParent(slots[i].transform); // set Item parent
                itemInstance.transform.localPosition = Vector2.zero; // set item local position (0, 0)
                itemPrefab.GetComponent<Image>().sprite = itemAdd.itemSprite; // set sprit of it
                break; // break out of loop when we add item
            }
        }
        
    }
}
