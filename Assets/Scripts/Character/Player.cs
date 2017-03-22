using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        public float speed=10f;
        public float jumpforce = 10f;
        private Rigidbody2D rb;

        public string jumpkey = "w";
        General.ListAnimation[] LS; //Probably migrate to Humus class
        private bool suspendMove = false;
        private float counter = 0;
        private bool flip;
        private bool onGround;
        private SpriteRenderer SR;

        private void Start()
        {
            SR = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            LS = gameObject.GetComponentsInChildren<General.ListAnimation>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                onGround = true;
            }

        }


        private void Move()
        {
            if (suspendMove == true) { return; }

            float sp = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(sp * speed, GetComponent<Rigidbody2D>().velocity.y);


            if (sp > 0.1f && flip) {
                //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                SR.flipX = false;
                flip = false; }
            if (sp < -0.1f && !flip) {
                SR.flipX = true; 
                //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                flip = true; }

            /*
            Vector2 MD = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MD -= new Vector2(transform.position.x, transform.position.y);

            //if (Mathf.Abs(MD.x) < 1f && Mathf.Abs(MD.x) > -1f) { return; }

            MD = new Vector2(MD.x, 0);
            if (Mathf.Abs(rb.velocity.x) < speed)
            {
                rb.AddForce(MD.normalized*speed);
            }
            //rb.velocity = new Vector2(MD.normalized.x*speed, rb.velocity.y);
            */

            if (!LS[0].inAni) { PlayAni(0); }
        }

        private void Jump() {

            if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpforce);
                onGround = false;
            }

        }

        private void PlayAni(int n, bool suspend = false)
        { // Still need to be smarter
            LS[0].PlayAnimation(n);
            counter = LS[0].currentAniTime();
            suspendMove = suspend;
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                Move();
            }
            if (Input.GetKey(jumpkey))
            {
                Jump();
            }
            if (Input.GetKey("f"))
            {
                PlayAni(1,true);
            }
            if (suspendMove)
            {
                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                }
                else { suspendMove = false; }
            }
        }


    }
}