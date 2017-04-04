using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class Player : MonoBehaviour
    {
        //private Humus H;
        public float speed=10f;
        private Rigidbody2D rb;

        public string movekey="q";
        public string jumpkey = "w";
        ListAnimation[] LS; //Probably migrate to Humus class

        public void ChangeControls(string movekey) {
            this.movekey = movekey;

        }

        private void Start()
        {
            //H = gameObject.GetComponent<Humus>();
            rb = GetComponent<Rigidbody2D>();
            LS = gameObject.GetComponentsInChildren<ListAnimation>();
        }

        private void Move()
        {
            Vector2 MD = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MD -= new Vector2(transform.position.x, transform.position.y);

            if (Mathf.Abs(MD.x) < 1f && Mathf.Abs(MD.x) > -1f) { return; }

            MD = new Vector2(MD.x, 0);
            if (Mathf.Abs(rb.velocity.x) < speed)
            {
                rb.AddForce(MD.normalized*speed*500);
            }
            //rb.velocity = new Vector2(MD.normalized.x*speed, rb.velocity.y);

            MoveAnimate();
        }

        private void Jump() {

            if (rb.IsTouchingLayers(1))
            {
                rb.velocity = new Vector2(rb.velocity.x,speed);
            }

        }


        private void StopMoveAnimate() { 
            foreach (ListAnimation anipart in LS)
            {
                anipart.repeat = false;
            }
        }
        //Not final, add support for individual animations or more smart animation control in general
        private void MoveAnimate()
        {
            foreach (ListAnimation anipart in LS)
            {
                if (!anipart.inAni)
                {
                    anipart.repeat = true;
                    anipart.PlayAll();
                }
            }
        }

        private void Update()
        {
            if (Input.GetKey(movekey))
            {
                Move();
            }
            if (Input.GetKey(jumpkey))
            {
                Jump();
            }
            else { StopMoveAnimate(); }
        }


    }
}