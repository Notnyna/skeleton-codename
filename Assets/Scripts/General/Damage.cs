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

        //private Character.Health CH; //monitor to not deal continuous damage?
        private bool destroy;
        ListAnimation LS;

        private void Start()
        {
            LS = GetComponent<ListAnimation>();
            if (LS != null) {
                LS.PlayAnimation(0);
                primeTime = LS.currentAniTime();
                //Debug.Log(primeTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (destroy) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player"))
            {
                DoDamage(cg);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Trigger! " + collision.name);
            if (destroy) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player"))
            {
                DoDamage(cg);
            }
        }

        private void DoDamage(Transform cg)
        {
            Character.Health h;
            if (cg.CompareTag("Monster")) { h = cg.GetComponentInParent<Character.Health>(); }
            else { h = cg.GetComponent<Character.Health>(); }
            //if (CH == h) { return; } //Maybe deal damage per frame ? Must save all targets though
            //if (CH == null) { CH = h; }
            if (h != null) { h.DealDamage(Dmg); }
            if (Apierce == 0)
            {
                Evaporate();
                return;
            }
            Apierce--;
            //Debug.Log("Doing damage!");
        }

        private void Evaporate()
        {
            if (LS != null)
            {
                LS.PlayAnimation(2,false); // 0 - fire, 1 - travel, 2 - destroy
                primeTime = LS.currentAniTime();
            }
            else { primeTime = 0.2f; }
            destroy = true;
        }

        private void Update()
        {
            if (primeTime > 0)
            {
                primeTime -= Time.deltaTime;
            }
            else { 
            if (LS != null & LS.AniIndex == 0) { LS.PlayAnimation(1, true); }
            if (destroy) { Destroy(this.gameObject); }
            }

            if (lifetime > 0)
            {
                lifetime -= Time.deltaTime;
            }
            else { destroy = true; }
        }

    }
}
