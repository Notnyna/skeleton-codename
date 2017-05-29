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
        General.Extender EX;
        //General.ListAnimation LS;
        //Rigidbody2D RB;
        public Character.Humus CH;
        public bool active;
        List<Collider2D> Dzones; 
        int cattack=0;
        bool sendtoH; //For changing Humus
        //public bool knockbackowner = true;
        Gun G;

        private void Start()
        {
            G= GetComponent<Gun>();
            MV = GetComponent<General.MoveAnimation>();
            EX = GetComponentInChildren<General.Extender>();
            if (MV == null) { Debug.Log("No mv for meleegun "+gameObject.name); }
            //LS = GetComponent<General.ListAnimation>();
            //RB = GetComponent<Rigidbody2D>();
            Dzones = new List<Collider2D>();
            foreach (Transform c in transform)
            {
                Collider2D[] ct = c.GetComponents<Collider2D>();
                if (ct != null) {
                    for (int i = 0; i < ct.Length; i++)
                    {
                        if (ct[i].isTrigger) { Dzones.Add(ct[i]); }
                    }
                    
                }
                
            }
            if (Dzones.Count == 0) {Dzones = null; Debug.Log("No DZ for " + gameObject.name); }
            if (!active) { DisableAttacks(); }
            //GetComponentInChildren<Collider2D>();
        }

        private void DisableAttacks(bool enable = false)
        {
            if (EX != null) { EX.ExtendPercent(0); }
            if (Dzones == null) { return; }
            //Debug.Log("disabling " + (cattack - 2));
            for (int i = 0; i < Dzones.Count; i++)
            {
                Dzones[i].enabled = enable;
            }
            //if ( active) { Dzones[0].enabled = true; return; }
        }

        private void EnableAttack()
        {
            if (EX != null) { EX.ExtendPercent(100); }
            if (Dzones == null) { return; }
            //if (active) { Dzones[0].enabled = false; }
            if (Dzones.Count > cattack)
            {
                Dzones[cattack].enabled = true;
                //Debug.Log("enabling "+(cattack-2));
            }
            else {
                Dzones[0].enabled = true;
            }
        }

        public void Fire()
        {
            if (!active) { return; }
            if (count0 > 0) { return; }
            
            if (MV != null )
            {
                MV.PlayAnimation(cattack + 2);
                EnableAttack();
            }
            else if (EX!=null) { count1 = (EX.cooldown+0.02f) * EX.maxext; count0 = count1 *2; EnableAttack(); }

            if (cattack == 2) {
                if (G != null) {
                    //G.enabled = true;
                    G.Fire(); //Test
                    //G.enabled= false;
                }
            }
            //if (count < 0.08f) { count = 2f; }
            cattack++;
            if (cattack > 2) { cattack = 0; }
            
            if (CH != null && CH.HeldItem==transform) { sendtoH = true; }
            if (MV != null) { count0 += MV.currentAniTime() + 0.2f; }
            //Debug.Log(cattack +"  "+ count0);
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
            //DisableAttacks();
            active = false;
        }

        private void OnTransformParentChanged()
        {
            OnEnable();
        }

        float count0;
        float count1;
        private void Update()
        {
            if (active)
            {
                if (count0 > 0)
                {
                    count0 -= Time.deltaTime;
                }
                else {
                    //CH.addItemPos = new Vector2(); 
                    sendtoH = false;
                    DisableAttacks(); 
                }
                if (EX != null)//Sure there is a better way but heck, im tired
                {
                    if (count0 > 0 && count1 > 0)
                    {
                        count1 -= Time.deltaTime;
                    }
                    else { DisableAttacks(); }
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (sendtoH) {
                //Vector2 Pt = CH.GetComponent<Rigidbody2D>().position;
                if (CH == null) { return; }
                if (MV == null) { return; }
                Vector2 mv = MV.GetcurrentAni();
                if (transform.lossyScale.x < 0) { mv = new Vector2(-mv.x,mv.y); }
                //Vector2 M = Vector2.Lerp(Vector2.zero,mv, Time.deltaTime/count); Menu.UsefulStuff.FromRotationToVector(transform.rotation.z,false)
                CH.addItemPos = mv  ;
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
                count0 = MV.currentAniTime();
            }
        }
    }
}
