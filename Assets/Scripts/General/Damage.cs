using UnityEngine;

namespace General
{
    /// <summary>
    /// Interacts only with Health? maybe some other.
    /// After dealing damage, it is destroyed. Not gameobject.
    /// 
    /// How to handle when hitting multiple colliders belonging to the same health?
    /// </summary>
    public class Damage : MonoBehaviour
    {
        public float Apierce = -1; //-1 means infinite
        public int Dmg = 1;
        public float primeTime=1f;

        private Character.Health CH; //current health
        private bool destroy;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (destroy) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player"))
            {
                Character.Health h = cg.GetComponent<Character.Health>();
                if (CH == null) { CH = h; }
                if (CH == h) { return; } //Maybe deal damage per frame ? Must save all targets though
                if (h != null) { h.DealDamage(Dmg); }
                if (Apierce < 0) { return; }
                if (Apierce == 0 )
                {
                    Evaporate();
                }
                Apierce--;
            }
        }

        private void Evaporate()
        {
            ListAnimation LS = GetComponent<ListAnimation>();
            if (LS != null)
            {
                LS.PlayAnimation(2); // 0 - fire, 1 - travel, 2 - destroy
                primeTime = LS.currentAniTime();
            }
            else { primeTime = 0.1f; }
            destroy = true;
        }

        private void Update()
        {
            if (primeTime > 0)
            {
                primeTime -= Time.deltaTime;
            }
            else if (destroy) { Destroy(this); }
        }

    }
}
