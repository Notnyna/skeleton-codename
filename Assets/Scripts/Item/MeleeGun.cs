using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    /// <summary>
    /// Works closely with Humus.
    /// Pretty useless otherwise.
    /// I consider this bad.
    /// For now all attacks go sequentally?
    /// MoveAnimation reservations:
    /// 0 - idle
    /// 1 - equip
    /// 2 - attack1
    /// 3 - attack2
    /// 4 - shootattack (List animation 1)
    /// </summary>
    public class MeleeGun : MonoBehaviour
    {
        General.MoveAnimation MV;
        //General.ListAnimation LS;
        //Rigidbody2D RB;
        public Character.Humus CH;
        bool active;
        List<Transform> Dzones;
        int cattack=2; //Start counting from 2 (equip is not attack ani)
        bool sendtoH; //For changing Humus

        private void Start()
        {
            MV = GetComponent<General.MoveAnimation>();
            //LS = GetComponent<General.ListAnimation>();
            //RB = GetComponent<Rigidbody2D>();
            Dzones = new List<Transform>();
            foreach (Transform c in transform)
            {
                Dzones.Add(c);
            }
            //GetComponentInChildren<Collider2D>();
        }

        private void DisableAttacks(bool enable = false)
        {
            if (Dzones == null) { return; }
            foreach (Transform d in Dzones)
            {
                d.gameObject.SetActive(enable);
            }

        }

        private void EnableAttack()
        {
            if (Dzones.Count > cattack-2)
            {
                Dzones[cattack - 2].gameObject.SetActive(true);
            }
            else {
                Dzones[0].gameObject.SetActive(true);
            }
        }

        public void Fire() 
        {
            if (!active) { return; }
            if (count > 0) { return; }
            if (MV.PlayAnimation(cattack)) {
                EnableAttack();
                count = MV.currentAniTime()+0.1f;
            } else {
                cattack = 2;
                EnableAttack();
                MV.PlayAnimation(cattack);
                count = MV.currentAniTime()+0.1f;
                //Fire();
                //return;
            }
            if (cattack == 4) {
                Gun G = GetComponent<Gun>();
                if (G != null) { G.enabled = true;
                    G.Fire();
                    G.enabled= false;
                    }
                }

            cattack++;
            sendtoH = true;
        }

        void OnEnable()
        {
            DisableAttacks();
            CH = GetComponentInParent<Character.Humus>();
            if (CH == null) {
                active = false;
                OnDisable();
            }
            else {
                active = true;
                DoEquipAni();
            }
        }

        private void OnDisable()
        {
            DisableAttacks();
            active = false;
        }

        private void OnTransformParentChanged()
        {
            OnEnable();
        }

        float count;
        private void Update()
        {
            if (active)
            {
                
                if (count > 0)
                {
                    count -= Time.deltaTime;
                }
                else { CH.addItemPos = new Vector2(); sendtoH = false; DisableAttacks(); }
            }
        }

        private void FixedUpdate()
        {
            if (sendtoH) {
                //Vector2 Pt = CH.GetComponent<Rigidbody2D>().position;
                Vector2 mv = MV.GetcurrentAni();
                if (transform.lossyScale.x < 0) { mv = new Vector2(-mv.x,mv.y); }
                //Vector2 M = Vector2.Lerp(Vector2.zero,mv, Time.deltaTime/count);
                CH.addItemPos = mv;//MV.GetcurrentAni(); 
            }
        }

        private void DoEquipAni()
        {
            if (MV == null) { return; }
            //Character.Player P = CH.GetComponent<Character.Player>();
            //Character.Humus H = P.GetComponent<Character.Humus>();
            if (CH.HeldItem == transform) //Only player can do animations for now
            {
                MV.PlayAnimation(1);
                //P.allowaiming = false;
                count = MV.currentAniTime();
            }
        }
    }
}
