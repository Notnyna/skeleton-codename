using UnityEngine;

namespace Item
{
    public class MeleeDamage : MonoBehaviour
    {
        public float punch=10;
        public int damage=1;
        public bool knockbackowner = true;
        MeleeGun MG;
        private void Start()
        {
            MG = GetComponentInParent<MeleeGun>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) { return; }
            if (MG != null) {
                //Check for the parent and all its transforms
                if (MG.CH != null) {
                    if (MG.CH.transform == collision.transform) { return; }
                    foreach (Transform t in MG.CH.transform)
                    {
                        if (collision.transform == t) { return; }
                    }
                }
            }
            
            Character.Health h;
            h = collision.GetComponent<Character.Health>();
            if (h != null)
            {
                float f = -1;
                if (transform.lossyScale.x < 0) { f = 1; }
                h.DealDamage(damage, collision.transform.position, f, punch*f);

                if (knockbackowner)
                {
                    if (MG != null && MG.CH != null)
                    {
                        Rigidbody2D pRB = MG.CH.GetComponent<Rigidbody2D>();
                        if (pRB != null)
                        {
                            pRB.AddForce(new Vector2((punch * -f) / 2, 0), ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
    }
}
