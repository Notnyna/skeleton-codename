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
        public float bleedmove= 10f;
        public float jumpforce = 10f;
        public string jumpkey = "w";
        public string actionbutton = "f";
        public string runbutton = "s";
        public string dropkey = "g";
        public bool available;
        private bool running;

        //public bool action { private set; get; }
        private Humus H;
        private Health HP;
        private Menu.MenuManager MM; //For calling inventory when picking up items

        private void Awake()
        {
            H = GetComponent<Humus>();
            if (H == null) { Debug.Log("No Humus in " + gameObject.name); }
            MM = FindObjectOfType<Menu.MenuManager>();
            if (MM == null) { Debug.Log("Cant find menu manager :("); }
            Inventory Inv = H.GetComponentInChildren<Inventory>(); 
            if (Inv != null) { Inv.OnChange += CallInventory; } else { Debug.Log("No inv :("); }
            HP = GetComponent<Health>();
            HP.OnDeath += Death;
            HP.HpChanged += HP_HpChanged;

        }

        bool slow;
        private void HP_HpChanged(Health who)
        {
            if (who.Dying())
            {
                if (!slow)
                {
                    float t = moveforce;
                    moveforce = bleedmove;
                    bleedmove = t;
                    slow = true;
                }
            }
            else {
                if (slow)
                {
                    float t = moveforce;
                    moveforce = bleedmove;
                    bleedmove = t;
                    slow = false;
                }
            }

            //if (who.GetPercentHP() > 20) { available = true; }
        }

        private void Death(Health who)
        {
            H.DoAnimation(5,false,true); // Death ani

            available = false;
            H.DropAll();
            Menu.GameMaster GM = FindObjectOfType<Menu.GameMaster>();
            if (enabled) { GM.SwitchPlayer(); }

            if (enabled) { Debug.Log("Game over!!"); enabled = false; }
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
                Item.MeleeGun MG = H.HeldItem.GetComponent<Item.MeleeGun>();
                if (MG != null) { MG.Fire(); }
                else
                {
                    Item.Gun G = H.HeldItem.GetComponent<Item.Gun>();
                    if (G != null) { G.Fire(GetComponent<Rigidbody2D>().velocity); }
                }
            } // I dont like this, I want it the other way around!
        }

        private bool allowflip=true;
        //public bool allowaiming = true; //For melee weapons
        private void Update()
        {
            if (Input.GetKeyDown(jumpkey)) { H.Jump(jumpforce);} 
            if (Input.GetKeyDown(actionbutton)) { DoAction(); }
            if (Input.GetKeyDown(dropkey)) { H.ReturnItem(); }
            if (Input.GetMouseButton(0)) { DoButtonAction(); }
            if (Input.GetKeyDown("q")) { H.HoldNextItem(true); } 
            if (Input.GetKeyDown("e")) { H.HoldNextItem(); }
            #region Movement and Aiming
            float axis = Input.GetAxis("Horizontal");
            if (axis != 0)
            {
                if (Input.GetKeyDown(runbutton)) { running = true; }
                if (running)
                {
                    if (H.Move(moveforce * axis * 2)) { H.DoAnimation(2, true); }
                }
                else
                {
                    if (H.Move(moveforce * axis, allowflip)) { H.DoAnimation(1, true); }
                }
            }
            else
            {
                running = false;
            } //endElse

            General.MoveAnimation MV = null;
            if (H.HeldItem != null)
            {
                
                MV = H.HeldItem.GetComponent<General.MoveAnimation>(); //This is pretty bad - make it event dependent
                if (MV != null && MV.AniIndex > 0) { return; }//allowaiming = false; } else { allowaiming = true; }

                float torot = Menu.UsefulStuff.MouseToPointRotation(H.HeldItem.transform.position);
                //FOR torot -90 to 90  When !flip 90 to -90(up)  When flip 270 to 90(up)
                float htorot = Menu.UsefulStuff.MouseToPointRotation(transform.position);
                //FOR htorot !flip -90(up) 90(down) | flip -90(up) -180to90(down)
                //Debug.Log(htorot);
                //torot += 180f;
                if (H.HeldItem.GetComponent<Rigidbody2D>() == null) { torot=0; }
                if (H.flip)
                {
                    if (Mathf.Abs(htorot) > 90)
                    {
                        allowflip = false;
                        H.HeldItem.rotation = Quaternion.Euler(0, 0, torot + 180);
                    }
                    else
                    {
                        if (H.flip)
                        {
                            if (!running)
                            {
                                allowflip = true;
                                H.Move(-1);
                                //Debug.Log("Flippin out");
                                allowflip = false;
                            }
                        }
                    }
                    
                }
                else
                {
                    if (Mathf.Abs(htorot) < 90)
                    {
                        allowflip = false;
                        H.HeldItem.rotation = Quaternion.Euler(0, 0, torot);
                    }
                    else
                    {
                        if (!H.flip)
                        {
                            if (!running)
                            {
                                allowflip = true;
                                //Debug.Log("Unflippin out");
                                H.Move(1);
                                allowflip = false;
                            }
                        }
                    }
                    
                }

               
            }
            else { allowflip = true; }

            //else
            // {
            //    if (H.HeldItem != null) { }//H.HeldItem.rotation = Quaternion.Euler(0, 0, 0); }
            //}
            #endregion


        }

    }
}