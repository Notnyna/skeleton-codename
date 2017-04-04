using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTestMovement : MonoBehaviour {

    public float speed,jumpingForce;
    public bool onGround, jumping, onPlatform;
    public Canvas cnv; 
    void Start () {
        speed = 5;
        jumpingForce = 5;
	}
	

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"  || collision.gameObject.tag == "button")
        {
            onGround = true;
            jumping = false;
            onPlatform = false;
            //PlatformMovement.buttonTouch = false;
        }

        /*if (collision.gameObject.tag == "button")
        {
            PlatformMovement.buttonTouch = true;
        }*/

        if (collision.gameObject.tag == "Platform")
        {
            onGround = true;
            onPlatform = true;
            jumping = false;
           
        }

        if(collision.gameObject.tag == "item" && inventory.invItemCount < 3) { 

            string itemSpriteName = collision.gameObject.GetComponent<SpriteRenderer>().sprite.name;
            Debug.Log(itemSpriteName+ " is taken");

            //Find inventory and Database
            inventory inv = GameObject.FindGameObjectWithTag("inventory").GetComponent<inventory>();
            itemDatabase itemDatabase = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<itemDatabase>();

            item willAdd = itemDatabase.GetItemBySpriteName(itemSpriteName);
            inv.AddItem(willAdd.itemID);

            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "item" && inventory.invItemCount >= 3) {
            Debug.Log("Inventory is full");
        }

    }

    void FixedUpdate()
    {
        float sp = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(sp * speed, GetComponent<Rigidbody2D>().velocity.y);

        if (sp > 0.1f)
            transform.localScale = new Vector2(2, 2);
        if (sp < -0.1f)
            transform.localScale = new Vector2(-2, 2);

       if(onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onGround = false;
            jumping = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpingForce);
        }
    }

     
}
