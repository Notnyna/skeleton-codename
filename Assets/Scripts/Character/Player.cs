using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        public float moveforce = 10f;
        public float jumpforce = 10f;
        public string jumpkey = "w";
        public string actionbutton = "f";
        public string dropkey = "g";

        private bool running;

        public bool action { private set; get; }
        private Humus H;

        private void Start()
        {
            H = GetComponent<Humus>();
            if (H==null)
            {
                Debug.Log("No Humus in "+gameObject.name);
            }
        }

        private void DoAction()
        {
            H.DoAnimation(3,false,true,0.3f);
            action = true;
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
        }

        private void Update()
        {
            float axis = Input.GetAxis("Horizontal");
            if (axis != 0)
            {
                if (Input.GetKeyDown("s"))  { running = true; }
                {
                    if (running)
                    {
                        if (H.Move(moveforce * axis * 2)) { H.DoAnimation(2, true); }
                    }
                    else {
                        if (H.Move(moveforce * axis)) { H.DoAnimation(1, true); }
                    }
                }
            }
            else running = false;
            if (Input.GetKeyDown(jumpkey)) { H.Jump(jumpforce);} 
            if (Input.GetKeyDown(actionbutton)) { DoAction(); } else { action = false; }
            if (Input.GetKeyDown(dropkey)) { H.ReturnItem(); }
            if (Input.GetKeyDown("q")) { H.HoldNextItem(true); }
            if (Input.GetKeyDown("e")) { H.HoldNextItem(); }
        }

    }
}