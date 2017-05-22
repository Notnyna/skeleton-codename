using UnityEngine;

namespace General
{
    /// <summary>
    /// Interacts only with Health? maybe some other.
    /// After dealing damage, it is destroyed. Not gameobject.
    /// How to handle when hitting multiple colliders belonging to the same health?
    /// </summary>
    public class Damage : MonoBehaviour
    {
        public float Apierce = -1; //-1 means infinite
        public int Dmg = 1;
        private float primeTime; //Depends on 0 animation time
        public float lifetime = 1;
        public float punch=0;
        public float FXspawn = 0.5f;

        //private Character.Health CH; //monitor to not deal continuous damage?
        private bool destroy;
        ListAnimation LS;
        SpawnFX FX;

        private void Start()
        {
            LS = GetComponent<ListAnimation>();
            FX = GetComponent<SpawnFX>();
            if (LS != null) {
                LS.PlayAnimation(0);
                primeTime = LS.currentAniTime();
                //Debug.Log(primeTime);
            }
            if (FX != null)  //FX reserved for bullets: 0 - Fire, 1 2 - Travel, 3 - Destroy (hit)
            {
                FX.DoFX(transform.rotation.eulerAngles.z,transform.position,30,10,new int[] { 0 },3);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (destroy) { return; }
            //if (collision.transform == transform) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player") | cg.CompareTag("Ground"))
            {
                DoDamage(cg, collision.transform.position); //maybe just put the bullet position?
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Trigger! " + collision.name);
            if (destroy) { return; }
            //if (collision.transform == transform.parent) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player") | cg.CompareTag("Ground"))
            {
                DoDamage(cg,collision.transform.position);
            }
        }

        private void DoDamage(Transform cg, Vector2 point)
        {
            Character.Health h;
            if (cg.CompareTag("Player") && primeTime>0) { return; }
            if (cg.CompareTag("Monster")) { h = cg.GetComponentInParent<Character.Health>(); }
            else { h = cg.GetComponent<Character.Health>(); }
            //if (CH == h) { return; } //Maybe deal damage per frame ? Must save all targets though
            //if (CH == null) { CH = h; }
            if (h != null) {
                if (punch != 0) {
                    //Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    bool flip = false; ;
                    if (transform.lossyScale.x < 0) { flip = true; }
                    h.GetComponent<Rigidbody2D>().AddForceAtPosition(Menu.UsefulStuff.FromRotationToVector(transform.rotation.eulerAngles.z,flip).normalized*-punch,cg.position, ForceMode2D.Impulse);

                }
                h.DealDamage(Dmg,point,transform.rotation.z);
            }
            if (Apierce == 0)
            {
                Evaporate();
                return;
            }
            Apierce--;
            if (FX != null) { FX.DoFX(transform.rotation.eulerAngles.z, transform.position, 10, 3, new int[] { 3 }, 1); }
            //Debug.Log("Doing damage!");
        }

        private void Evaporate()
        {
            if (LS != null)
            {
                LS.PlayAnimation(2,true); // 0 - fire, 1 - travel, 2 - destroy
                primeTime = LS.currentAniTime();
            }
            else { primeTime = 0.2f; }
            destroy = true;
            if (FX != null) { FX.DoFX(transform.rotation.eulerAngles.z, transform.position, 10, 3, new int[] { 3 }, 1); }
        }

        private float fxcount;

        private void Update()
        {
            if (primeTime > 0)
            {
                primeTime -= Time.deltaTime;
            }
            else { 
            if (LS != null && LS.AniIndex == 0) { LS.PlayAnimation(1, true); }
            if (destroy) { Destroy(this.gameObject); }
            } //endelse

            if (lifetime > 0)
            {
                //FXspawn - how many spawn before death. 1: lifetime-(lifetime/fxs)  2: lifetime-(lifetime/fxs)*(fxs-1) ... 
                //if (FX!=null && ( lifetime - (lifetime / FXspawn) < lifetime) ) {
                //    
                //    FXspawn = FXspawn / (FXspawn - 1);
                //}
                lifetime -= Time.deltaTime;
            }
            else { Evaporate(); }

            if (FX != null)
            {
                fxcount += Time.deltaTime;
                if (fxcount>FXspawn) {
                    FX.DoFX(transform.rotation.eulerAngles.z, transform.position, 5, 1, new int[] { 1, 2 }, 1);
                    fxcount = 0;
                }
            }
        }

    }
}
