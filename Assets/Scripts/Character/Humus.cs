using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{

    /// <summary>
    /// Is not for generating a 'humus'.
    /// Handles the surface level part and component management to act as a global character interactor (Think 2d).
    /// For a more visual description it is a smaller component, possibly a live being that the ground consists of.
    /// For a more graphical description it is a continuously decomposing pile of mud.
    /// -Used by staticposoverride.
    /// </summary>
    public class Humus : MonoBehaviour
    {
        //private List<Transform> bodyparts;
        //private List<Transform> inventory;
        private Rigidbody2D RB;
        //private SpriteRenderer SR;
        private General.ListAnimation LS;
        private Inventory Inv;

        public Transform HeldItem;
        public Vector2 ItemholdLocation; //default hold
        //private FixedJoint2D HIJoint;  //Would be much better

        public int jumps = 1;
        private int cjumps;
        private bool onGround;
        private bool stopMove;
        public bool flip { get; private set; } //Scale is >0 if false, 0< if true
        private bool dieflag;


        private float counter;

        private void Awake()
        {
            //bodyparts = new List<Transform>();
            //inventory = new List<Transform>();
            RB = GetComponent<Rigidbody2D>();
            //SR = GetComponent<SpriteRenderer>();
            LS = GetComponent<General.ListAnimation>();
            Inv = GetComponentInChildren<Inventory>();
            //HIJoint = new FixedJoint2D();
            //UpdateParts();
        }

        public bool Move(float force, bool allowflip=true) {
            if (stopMove) { return false; } //RB.velocity = new Vector2(0,RB.velocity.y); 
            if (RB == null) { return false; }
            if (!onGround)
            {
                if (Mathf.Abs(RB.velocity.x) < Mathf.Abs(force))
                {
                    RB.AddForce(new Vector2(force * 2, 0), ForceMode2D.Impulse);
                }
                //RB.velocity = new Vector2(force, RB.velocity.y);
            }
            else {
                if (Mathf.Abs(RB.velocity.x) < Mathf.Abs(force))
                {
                    RB.AddForce(new Vector2(force*4, 0), ForceMode2D.Impulse);
                }
                //RB.velocity = new Vector2(force, 0);
            }
            if (!allowflip) { return true; }
            if (force < 0 & flip) {
                //if (SR == null) {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
                ItemholdLocation.x = -ItemholdLocation.x;
                //}
                //else SR.flipX = true;
                flip = false;
            }
            if (force > 0 & !flip) {
                //if (SR == null) {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
                ItemholdLocation.x = Mathf.Abs(ItemholdLocation.x);
                //}
                //else  SR.flipX = false;
                flip = true;
            }
            return true;
        }

        public void Jump(float jf)
        {
            //if (cjumps < 1 & !onGround) { onGround = true; cjumps++; }
            if (jumps>1 & cjumps < jumps & !onGround) { onGround = true; }
            if (!onGround | stopMove) { return; }
            //RB.AddForce(new Vector2(0,jf));
            RB.velocity = new Vector2(RB.velocity.x, jf);
            onGround = false;
            cjumps++;
        }

        public void Die(float time=5)
        {
            DoAnimation(5,false,true,time);
            dieflag = true;
        }

        public float DoAnimation(int ani, bool allowMove = true, bool repeat = false, float suspendtime=-1)
        {
            if (LS == null) {  return DoAnimationParts(ani, allowMove, repeat); }
            LS.PlayAnimation(ani,repeat);
            if (allowMove) { return LS.currentAniTime(); }
            if (suspendtime < 0)
            {
                suspendMove(LS.currentAniTime());
                return LS.currentAniTime();
            }
            else { suspendMove(suspendtime); return LS.currentAniTime(); }
        }

        private float DoAnimationParts(int ani, bool allowMove = true, bool repeat = false)
        {
            General.ListAnimation cLS;
            float time=0;
            //Gets the maximum animation time from each part for current animation
            foreach (Transform child in transform)
            {
                cLS = child.GetComponent<General.ListAnimation>();
                if (cLS != null)
                {
                    cLS.PlayAnimation(ani,repeat);
                    float atime = cLS.currentAniTime();
                    if (atime>time) { time = atime; }
                    cLS = null;
                }
            }
            return time;
            /*
            if (LS == null) return 0;
            LS.PlayAnimation(ani,repeat);
            if (allowMove) { return LS.currentAniTime(); }
            suspendMove(LS.currentAniTime());
            return LS.currentAniTime(); 
            */
        }

        public int CurrentAnimation(bool sprite = false, int part=-1) {
            if (LS == null)
            {
                General.ListAnimation cLS;

                if (part > 0 && transform.GetChild(part)!=null) {
                    cLS = transform.GetChild(part).GetComponent<General.ListAnimation>();
                    if (cLS == null) { return -1; }
                    if (sprite) { return cLS.currentSprite(); } else return cLS.AniIndex;
                }
                
                int s = -1;
                foreach (Transform child in transform)
                {
                    cLS = child.GetComponent<General.ListAnimation>();
                    if (cLS != null)
                    {
                        if (sprite)
                        {
                            if (cLS.currentSprite() > s) { s = cLS.currentSprite(); }
                        }
                        else { if (cLS.AniIndex > s) { s = cLS.AniIndex; } }
                        cLS = null;
                    }
                }
                return s;
            }
            else
            {
                if (sprite)
                {
                    return LS.currentSprite();
                }
                else return LS.AniIndex;
            }
        }

        private void suspendMove(float time) {
            stopMove = true;
            //Debug.Log("Stopping for " + time);
            counter = time;
        }

        #region Item/Inventory
        public Transform[] GetInventory()
        {
            if (Inv == null) { return null; }
            return Inv.GetInventory();
        }

        public bool TakeItem(Transform item)
        {
            if (Inv == null) { return false; }
            if (!Inv.TakeItem(item)) { return false; }
            //item.localPosition = ItemholdLocation;
            item.transform.localScale = new Vector3(1,1,1);
            //item.localPosition = ItemholdLocation/transform.localScale.x;
            //Debug.Log(iscale.ToString());
            //if ((flip & iscale.x > 0) | (!flip & iscale.x < 0)) { iscale = new Vector3(-iscale.x, iscale.y, 1); Debug.Log("flipping item"); } 
            HoldItem(Inv.currentselect);
            DoAnimation(3, false);
            return true;
        }

        public bool ReturnItem(Transform item)
        {
            if (Inv == null) { return false; }
            if (item == HeldItem) { HeldItem = null; }
            return Inv.ReturnItem(item);
        }

        public Transform ReturnItem(int i=-1)
        {
            if (Inv == null) { return null; }
            if (i < 0) { HeldItem = null; }
            Transform h = Inv.ReturnItem(i);
            //if (h == HeldItem) { HeldItem = null; }
            return h;
        }

        public int GetInventorySpace()
        {
            if (Inv == null) { return 0; }
            return Inv.maxitems;
        }

        public void HoldNextItem(bool back=false)
        {
            if (Inv == null) { return; }
            int h = Inv.currentselect;
            if (!back) { h++; } else { h--; }
            if (h >= Inv.maxitems) { h = 0; } else if (h < 0) { h = Inv.maxitems-1; }
            HoldItem(h);
        }

        public void HoldItem(int i)
        {
            if (Inv == null) { return; }
            Transform holdnew = Inv.SetHeld(i);
            if (holdnew != HeldItem & HeldItem!=null) { HeldItem.gameObject.SetActive(false); }
            HeldItem = holdnew;
            if (HeldItem != null)
            {
                addItemPos = new Vector2();
                HeldItem.localPosition = ItemholdLocation / transform.localScale.x;
                HeldItem.gameObject.SetActive(true);
            }
        }

        public int HoldIndex()
        {
            if (Inv == null) { return -1; }
            return Inv.currentselect;
        }

        public void DropAll()
        {
            if (Inv == null) { return; }
            for (int i = 0; i < Inv.maxitems; i++)
            {
                Inv.ReturnItem(i);
            }
            HeldItem = null;
        }
        #endregion

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Detection") && !collision.isTrigger)
            {
                if (!onGround & !stopMove) { onGround = true; cjumps = 0; suspendMove(0.1f); }
            }
        }

        private List<Transform> Triggers = new List<Transform>();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Can jump only if trigger enters, not anything else
            if (!CompareTag("Detection") && !collision.isTrigger) { Triggers.Add(collision.transform); }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!CompareTag("Detection") && !collision.isTrigger) { Triggers.Remove(collision.transform); }
            if (Triggers.Count == 0)  { onGround = false; }
        }

        private void Update()
        {
            if (stopMove)
            {
                if (counter > 0)
                {
                    counter -= Time.deltaTime;
                }
                else { stopMove = false; if (dieflag) { Destroy(gameObject); } }
            }
             //Might make the item glitch when game lags, have to check

            //if (!onGround) { RB.gravityScale = 1.4f; } else { RB.gravityScale = 1; }
        }
        
        public Vector2 addItemPos;
        
        private void FixedUpdate()
        {
            if (HeldItem != null) {
                Rigidbody2D HRB = HeldItem.GetComponent<Rigidbody2D>(); //Should make global in class
                if (HRB != null) {
                    HRB.MovePosition(ItemholdLocation + new Vector2(transform.position.x, transform.position.y) + addItemPos
                        );
                }
                //if (!HeldItem.gameObject.activeSelf) { HeldItem.gameObject.SetActive(true); }
                else
                {
                    HeldItem.transform.localPosition = ItemholdLocation;
                    // Should not be item in the first place really.
                }
            }
        }

    }
}
