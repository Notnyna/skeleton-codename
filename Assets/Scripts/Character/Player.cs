using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        public float moveforce=10f;
        public float jumpforce = 10f;
        public string jumpkey = "w";
        public string actionbutton = "f";

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
            H.DoAnimation(2,false);
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
                H.Move(moveforce * axis);
                if (H.CurrentAnimation() < 2)
                {
                    H.DoAnimation(1, true);
                }
            }
            if (Input.GetKeyDown(jumpkey)) { H.Jump(jumpforce);} 
            if (Input.GetKeyDown(actionbutton)) { DoAction(); } else { action = false; }
        }

    }
}