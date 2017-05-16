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
        private SpriteRenderer SR;
        private General.ListAnimation LS;

        public int heldItem { get; private set; } 
        public Transform HeldItem;
        public int jumps = 1;
        private int cjumps;
        private bool onGround;
        private bool stopMove;
        private bool flip=true;
        private bool dieflag;
        public Vector2 ItemholdLocation;

        private float counter;

        private void Start()
        {
            //bodyparts = new List<Transform>();
            //inventory = new List<Transform>();
            RB = GetComponent<Rigidbody2D>();
            SR = GetComponent<SpriteRenderer>();
            LS = GetComponent<General.ListAnimation>();
            heldItem = 0;
            //UpdateParts();
        }

        public bool Move(float force) {
            if (stopMove) { RB.velocity = new Vector2(0,RB.velocity.y); return false; }
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
            
            if (force > 0 && flip) {
                if (SR == null) { transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1); }
                else SR.flipX = true;
                flip = false;
            }
            if (force < 0 && !flip) {
                if (SR == null) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1); }
                else  SR.flipX = false;
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
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return null; }
            return Inv.GetInventory();
        }

        public bool TakeItem(Transform item)
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return false; }
            Inv.TakeItem(item);
            return false;
        }

        public bool ReturnItem(Transform item)
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return false; }
            return Inv.ReturnItem(item);
        }

        public Transform ReturnItem(int i=-1)
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return null; }
            return Inv.ReturnItem(i);
        }

        public int GetInventorySpace()
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return 0; }
            return Inv.maxitems;
        }

        public void HoldNextItem(bool back=false)
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return; }
            if (!back) { if (heldItem == Inv.maxitems-1) { heldItem = -1; } HoldItem(heldItem + 1); } //next item
            else { if (heldItem == 0) { heldItem = Inv.maxitems; } HoldItem(heldItem - 1); } // previous item
            /*
            for (int i = 0; i < I.Length; i++)
            {
                if (I[i] == HeldItem) {
                    if (!back) { if (i == I.Length) { i = -1; } HoldItem(i + 1); } //next item
                    else { if (i == 0) { i = I.Length+1; } HoldItem(i-1); } // previous item
                    return;
                }
            }
            */
            
        }

        public void HoldItem(int i)
        {
            Inventory Inv = GetComponentInChildren<Inventory>();
            if (Inv == null) { return; }
            heldItem = i;
            HeldItem = Inv.SetHeld(i);
        }
        #endregion

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Detection"))
            {
                if (!onGround & !stopMove) { onGround = true; cjumps = 0; suspendMove(0.1f); }
            }
        }

        private List<Transform> Triggers = new List<Transform>();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Can jump only if trigger enters, not anything else
            if (!CompareTag("Detection")) { Triggers.Add(collision.transform); }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!CompareTag("Detection")) { Triggers.Remove(collision.transform); }
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
            //if (!onGround) { RB.gravityScale = 1.4f; } else { RB.gravityScale = 1; }
        }

    }
}
