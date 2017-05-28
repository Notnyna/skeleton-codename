using UnityEngine;

namespace Item
{
    /// <summary>
    /// To use safely prime time of bullet damage must not hit the user.
    /// 
    /// Gun animation reservations:
    /// 0-Idle/ammo full
    /// 1-fire
    /// 2-ammo empty
    /// 3-ammo mid
    /// 4-ammo low
    /// 
    /// </summary>
    public class Gun : MonoBehaviour
    {
        public GameObject Bullet;
        public float bulletoffset=1;
        public int clip; //Can be improved by lots and lots and lots
        public float reloadtime;
        public float cooldown;
        public bool autoreload;
        //public int ammo; //The total bullets you can shoot, including all clips
        //public int maxammo;

        private int cclip; //current clip
        private float counter;
        private bool canfire;
        public float fireforce = 10;
        General.ListAnimation LS;
        General.MoveAnimation MV;

        private void Start()
        {
            MV = GetComponent<General.MoveAnimation>();
            LS = GetComponent<General.ListAnimation>();
            if (Bullet == null) { Debug.Log("Shooting nothing!"); }
            //if (ammo > 0) { Reload(); }
        }

        public void Fire(Vector2 v=new Vector2())//float direction) Should implement rotation if dont want to fire only according to gun
        {
            if (!canfire) { return; }
            GameObject b = Instantiate(Bullet);
            //float flip = -1;
            //if (transform.lossyScale.x < 0) { flip = 1; }
            bool flip=false;
            if (transform.lossyScale.x > 0) { flip = true; }
            Vector2 Boff = Menu.UsefulStuff.FromRotationToVector(transform.rotation.eulerAngles.z,flip);
            b.transform.position = new Vector3(transform.position.x+Boff.x*bulletoffset, transform.position.y+Boff.y*bulletoffset,transform.position.z);
            b.transform.rotation = transform.rotation;

            Rigidbody2D brb=b.GetComponent<Rigidbody2D>();

            if (transform.lossyScale.x < 0) { b.transform.localScale = new Vector2(-b.transform.localScale.x, b.transform.localScale.y); }

            Rigidbody2D pRB = transform.parent.GetComponentInParent<Rigidbody2D>();//kinda strict and bad?
            Vector2 add = Vector2.zero;
             if (pRB != null) {

                add = pRB.velocity * brb.mass/2;
                pRB.AddForce(-Boff * fireforce, ForceMode2D.Impulse);
            }

            brb.AddForce( //Add impulse to bullet!
                (Boff * fireforce) +add
                , ForceMode2D.Impulse);
            

            cclip--;
            firing = true;
            counter = cooldown;
            //if (autoreload) { rcounter = reloadtime; }
            canfire = false;
            FireAni();
        }

        public void ReloadAni()
        {
            if (LS == null) { return; }
            //if (cclip == 0 | clip==0) { LS.PlayAnimation(4); return; }
            for (int i = 1; i < 4; i++)
            {
                //Debug.Log((clip / 4) * i);
                if (cclip <= (clip / 4) * i) { LS.PlayAnimation(i+1); return; }
            }
        }

        private void FireAni()
        {
            if (LS==null) { return; }
            LS.PlayAnimation(1);
        }

        public void Reload(int i=-1)
        {
            if (i < 0) { cclip = clip;canfire = false; }
            else {
                if (cclip+i <= clip)
                {
                    cclip += i;   canfire = false;
                }
            }
            rcounter = reloadtime;
        }

        private bool firing;
        private float rcounter;

        private void Update()
        {
            if (counter > 0) {
                counter -= Time.deltaTime;
                firing = false;
            } else {
                if (cclip > 0) { canfire = true; }
            }

            if (rcounter > 0)
            {
                rcounter -= Time.deltaTime;
            }
            else {
                if (!firing & autoreload) { Reload(1); }
            }

            ReloadAni();
            
        }

        private void OnEnable()
        {
            counter = 0;
            if (autoreload) { Reload(2); }
            DoEquipAni();
        }

        public int GetAmmoPercent()
        {
            return Mathf.FloorToInt(((float)cclip / (float)clip) * 100f);
        }

        private void DoEquipAni()
        {
            if (MV == null) { return; }
            //Character.Player P = GetComponentInParent<Character.Player>();
            Character.Humus H = GetComponentInParent<Character.Humus>();
            if (H!=null && H.HeldItem==transform) //Only player can do animations for now
            {
                MV.PlayAnimation(1);
                counter = MV.currentAniTime();
                canfire = false;
            }
        }

    }
}