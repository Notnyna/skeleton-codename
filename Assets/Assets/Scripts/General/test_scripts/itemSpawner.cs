using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemSpawner : MonoBehaviour {

    public GameObject character;
    public GameObject obj;
    public static SpriteRenderer spRen;
    public static string droppedItemName;
    public static bool dropItem;

	void Start () {
        dropItem = false;
        droppedItemName = null;
        spRen = obj.GetComponent<SpriteRenderer>();
        spRen.sprite = null;
        //obj.tag = "item";
    }
	
	void Update () {
        if(dropItem) {

            spRen.sprite = Resources.Load<Sprite>("TestItems/" + droppedItemName);

            DestroyImmediate(obj.GetComponent<PolygonCollider2D>(), true);
            obj.AddComponent<PolygonCollider2D>();

            Instantiate(obj, new Vector3(character.transform.position.x + 2, character.transform.position.y, character.transform.position.z), Quaternion.identity);
            inventory.invItemCount--;
            //Debug.Log(inventory.invItemCount);
            dropItem = false;
        }
    }

   

}
