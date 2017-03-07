using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTestMovement : MonoBehaviour {

    public float speed,jumpingForce;
    public bool onGround,jumping;
    //Rigidbody2D weight;

    void Start () {
        speed = 5;
        jumpingForce = 5;
        //weight = GetComponent<Rigidbody2D>();	
	}
	

	void Update () {
        /*if (jumping)
        {
           PlatformMovement.buttonTouch = false;
        }*/
          
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"  || collision.gameObject.tag == "button")
        {
            onGround = true;
            jumping = false;
            //PlatformMovement.buttonTouch = false;
        }

        if (collision.gameObject.tag == "button")
        {
            //PlatformMovement.buttonTouch = true;
        }

        if (collision.gameObject.tag == "Platform")
        {
            onGround = true;
        }
        
    }

    void FixedUpdate()
    {
        float sp = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(sp * speed, GetComponent<Rigidbody2D>().velocity.y);

        if (sp > 0.1f)
            transform.localScale = new Vector2(5, 5);
        if (sp < -0.1f)
            transform.localScale = new Vector2(-5, 5);

       if(onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onGround = false;
            jumping = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpingForce);
        }
    }
}
