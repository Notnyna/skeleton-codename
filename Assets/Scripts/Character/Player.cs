using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        public delegate void DoingAction(Transform who);
        public event DoingAction OnAction;
        public delegate void ActivateItem(Vector2 Point);
        public event ActivateItem OnActivate;

        public float moveforce = 10f;
        public float jumpforce = 10f;
        public string jumpkey = "w";
        public string actionbutton = "f";
        public string runbutton = "s";
        public string dropkey = "g";

        private bool running;

        //public bool action { private set; get; }
        private Humus H;
        private Menu.MenuManager MM; //For calling inventory when picking up items

        private void Awake()
        {
            H = GetComponent<Humus>();
            if (H == null) { Debug.Log("No Humus in " + gameObject.name); }
            MM = FindObjectOfType<Menu.MenuManager>();
            if (MM == null) { Debug.Log("Cant find menu manager :("); }
            Inventory Inv = H.GetComponentInChildren<Inventory>(); 
            if (Inv != null) { Inv.OnChange += CallInventory; } else { Debug.Log("No inv :("); }

        }

        private void CallInventory(Transform item, int index, bool removed)
        {
            MM.ChangeMenu(2,true);
        }

        private void DoAction()
        {
            if (OnAction != null) { OnAction(transform); }
            //H.DoAnimation(3,false,true,0.3f);
            //action = true;
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

        private void DoButtonAction()
        {
            if (OnActivate != null) { OnActivate(Camera.main.ScreenToWorldPoint(Input.mousePosition)); }
            if (H.HeldItem !=null) {
                Item.Gun G = H.HeldItem.GetComponent<Item.Gun>();
                if (G != null) { G.Fire(); }

            } // I dont like this, I want it the other way around!
        }

        private void Update()
        {
            float axis = Input.GetAxis("Horizontal");
            if (axis != 0)
            {
                if (Input.GetKeyDown(runbutton)) { running = true; }
                {
                    if (running)
                    {
                        if (H.Move(moveforce * axis * 2)) { H.DoAnimation(2, true); }
                    }
                    else
                    {
                        if (H.Move(moveforce * axis)) { H.DoAnimation(1, true); }
                    }
                }
            }
            else {
                running = false;
                if (Input.GetKeyDown(runbutton)) {
                    H.DoAnimation(3, false,true); //How about implement full mouse aiming!?

                }

            }
            if (Input.GetKeyDown(jumpkey)) { H.Jump(jumpforce);} 
            if (Input.GetKeyDown(actionbutton)) { DoAction(); }
            if (Input.GetKeyDown(dropkey)) { H.ReturnItem(); }
            //if (Input.GetKeyDown(aimbutton)) { H.DoAnimation(3, false, true, 0.4f); }
            if (Input.GetKeyDown("q")) { H.HoldNextItem(true); }
            if (Input.GetKeyDown("e")) { H.HoldNextItem(); }
            if (Input.GetMouseButton(0)) { DoButtonAction(); }
        }

    }
}