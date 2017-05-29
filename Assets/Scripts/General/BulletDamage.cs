using UnityEngine;

namespace General
{
    /// <summary>
    /// FX reserved for bullets: 0 - Fire, 1 2 - Travel, 3 - Destroy (hit)
    /// Interacts only with Health? maybe some other.
    /// How to handle when hitting multiple colliders belonging to the same health?
    /// </summary>
    public class BulletDamage : MonoBehaviour
    {
        public float Apierce = -1; //-1 means infinite
        public int Dmg = 1;
        private float primeTime; //Depends on 0 animation time
        public float lifetime = 1;
        public float punch=0;
        public float FXspawn = 0.5f; // Time for fx to spawn again

        //private Character.Health CH; //monitor to not deal continuous damage?
        private bool destroy;
        ListAnimation LS;
        SpawnFX FX;
        SpriteRenderer SR;
        Vector2 direction; // for now should not change during travel

        private void Start()
        {
            LS = GetComponent<ListAnimation>();
            FX = GetComponent<SpawnFX>();
            if (LS != null) {
                LS.PlayAnimation(0);
                primeTime = LS.currentAniTime();
                //Debug.Log(primeTime);
            }

            bool f = false;
            if (transform.lossyScale.x > 0) { f = true; }
            direction = Menu.UsefulStuff.FromRotationToVector(transform.rotation.eulerAngles.z, f);

            if (FX != null)  
            {
                FX.DoFX(direction,transform.position,30,10,new int[] { 0 },3);
            }
            SR=GetComponent<SpriteRenderer>();
            SR.sortingOrder=Random.Range(-5,6);

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (destroy) { return; }
            //if (collision.transform == transform) { return; }
            Transform cg = collision.transform;
            //if (cg.CompareTag("Monster") | cg.CompareTag("Player") | cg.CompareTag("Ground"))
           // {
                DoDamage(cg, collision.transform.position); //maybe just put the bullet position?
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) { return; }
            //Debug.Log("Trigger! " + collision.name);
            if (destroy) { return; }
            //if (collision.transform == transform.parent) { return; }
            Transform cg = collision.transform;
            //if (cg.CompareTag("Monster") | cg.CompareTag("Player") | cg.CompareTag("Ground"))
            //{
                DoDamage(cg,collision.transform.position);
            //}
        }

        public int accuracy = 10;
        private void DoDamage(Transform cg, Vector2 point)
        {
            Character.Health h;
            if (cg.CompareTag("Player") && primeTime>0) { return; }

            if (Random.Range(0, accuracy) == 0) { return; }

            h = cg.GetComponent<Character.Health>();
            bool pierce = true;
            if (h != null) {

                pierce =h.DealDamage(Dmg, transform.position, direction,punch);
            }

            /*if (punch != 0) {
                //Rigidbody2D rb = GetComponent<Rigidbody2D>();
                float f = 1 ;
                bool flip=false;
                if (transform.lossyScale.x < 0) { f=-1; flip = true; }
                //Replace with health punch
                h.GetComponent<Rigidbody2D>().AddForceAtPosition(
                    Menu.UsefulStuff.FromRotationToVector(transform.rotation.eulerAngles.z,flip).normalized*
                    -punch,cg.position
                    , ForceMode2D.Impulse);
            }*/

            

            if (!pierce | Apierce < 1)
            {
                Evaporate();
                return;
            }
            Apierce--;
            if (FX != null) { FX.DoFX(-direction, transform.position, 10, 3, new int[] { 3 }, 1); }
            //Debug.Log("Doing damage!");
        }

        private void Evaporate()
        {
            if (Apierce < 0) { return; }
            if (LS != null ) 
            {
                LS.PlayAnimation(2,true); // 0 - fire, 1 - travel, 2 - destroy
                primeTime = LS.currentAniTime();
            }
            else { primeTime = 0.2f; }
            destroy = true;
            if (FX != null) { FX.DoFX(direction, transform.position, 10, 3, new int[] { 3 }, 1); }
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
                //if (Random.Range(0, 3) == 0) { Miss(); }
                lifetime -= Time.deltaTime;
            }
            else { if (!destroy) { Evaporate(); } }

            if (FX != null)
            {
                fxcount += Time.deltaTime;
                if (fxcount>FXspawn) {
                    FX.DoFX(direction, transform.position, 5, 1, new int[] { 1, 2 }, 1);
                    fxcount = 0;
                }
            }
        }

        //void Miss()
       // {
        //    if (gameObject.layer == 2) { gameObject.layer = 0; }
       //     else { gameObject.layer = 2; } 
       // }

    }
}
