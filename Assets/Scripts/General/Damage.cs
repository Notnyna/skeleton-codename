using UnityEngine;

namespace General
{
    public class Damage : MonoBehaviour
    {
        public float Apierce = 0;
        public int Dmg = 1;
        public float primeTime=1f;

        private bool destroy;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (destroy) { return; }
            Transform cg = collision.transform;
            if (cg.CompareTag("Monster") | cg.CompareTag("Player"))
            {
                Character.Health h = cg.GetComponent<Character.Health>();
                if (h != null) { h.DealDamage(Dmg); }
                Apierce--;
                if (Apierce < 0 )
                {
                    Evaporate();
                }
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
            else if (destroy) { Destroy(gameObject); }
        }

    }
}
