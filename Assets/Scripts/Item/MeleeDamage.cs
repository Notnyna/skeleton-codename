using UnityEngine;

namespace Item
{
    public class MeleeDamage : MonoBehaviour
    {
        public float punch=10;
        public int damage=1;
        MeleeGun MG;
        private void Start()
        {
            MG= GetComponentInParent<MeleeGun>();
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
            if (h == null) { return; }
            if (punch != 0)
            {
                float f = -1;
                //Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (transform.lossyScale.x < 0) { f = 1; }
                h.DealDamage(damage, collision.transform.position, transform.rotation.z);
                //Do punch (health)
            }
           // if (FX != null) { FX.DoFX(transform.rotation.eulerAngles.z, transform.position, 10, 3, new int[] { 3 }, 1); }
        }
    }
}
