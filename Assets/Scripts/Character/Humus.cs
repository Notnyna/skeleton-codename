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

        public int jumps = 1;
        public int cjumps;
        public bool onGround;
        private bool stopMove;
        public bool flip { get; private set; }

        private float counter;

        private void Awake()
        {
            //bodyparts = new List<Transform>();
            //inventory = new List<Transform>();
            RB = GetComponent<Rigidbody2D>();
            SR = GetComponent<SpriteRenderer>();
            LS = GetComponent<General.ListAnimation>();
            
            //UpdateParts();
        }

        #region Containers 
        private void CompareAdd(List<Transform> L, Transform parent)
        {
            Transform[] parts = parent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < parts.Length; i++)
            {
                if (!L.Contains(parts[i]))
                {
                    L.Add(parts[i]);
                    //Debug.LogFormat("{0} Added", parts[i].name);
                }
            }

        }

        public void UpdateParts()
        {
            //GetComponentInChildren<General.StaticPos>().UpdatePositions();
            foreach (Transform p in transform)
            {
                //if (p.name == "bodyparts") { CompareAdd(bodyparts, p); }
                //if (p.name == "inventory") { CompareAdd(inventory, p); }
            }
        }

        public Transform GetByName(string n, int type)
        {
            List<Transform> S = null;
            //if (type == 0) { S = bodyparts; }
            //if (type == 1) { S = inventory; }

            if (S != null) {
                foreach (Transform item in S)
                {
                    if (string.Equals(n, item.name)) {  return item;   }
                }
            }
            return null;
        }

        public T GetByType<T>()
        {
            return gameObject.GetComponentInChildren<T>();
        }
        #endregion

        public void Move(float force) {
            if (stopMove) { return; }
            if (RB == null || SR == null) { return; }

            if (!onGround)
            {
                RB.AddForce(new Vector2(force/2, 0));
            }
            else { RB.AddForce(new Vector2(force, 0)); }

            if (force > 0 && flip) {
                SR.flipX = false;
                flip = false;
            }
            if (force < 0 && !flip) {
                SR.flipX = true;
                flip = true;
            }
            
        }

        public void Jump(float jf)
        {
            if (cjumps < 1 & !onGround) { onGround = true; cjumps++; }
            if (cjumps < jumps & !onGround) { onGround = true; }
            if (!onGround | stopMove) { return; }
            //RB.AddForce(new Vector2(0,jf));
            RB.velocity = new Vector2(RB.velocity.x, jf);
            onGround = false;
            cjumps++;
        }

        public float DoAnimation(int ani, bool allowMove = true, bool repeat = false)
        {
            if (LS == null) return 0;
            LS.PlayAnimation(ani,repeat);
            if (allowMove) { return LS.currentAniTime(); }
            suspendMove(LS.currentAniTime());
            return LS.currentAniTime(); 
        }

        public int CurrentAnimation(bool sprite = false) { if (sprite) { return LS.currentSprite(); } else return LS.AniIndex; }

        private void suspendMove(float time) {
            stopMove = true;
            counter = time;
        }

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
                else { stopMove = false; }
            }
            if (!onGround) { RB.gravityScale = 1.4f; } else { RB.gravityScale = 1; }
        }

    }
}
