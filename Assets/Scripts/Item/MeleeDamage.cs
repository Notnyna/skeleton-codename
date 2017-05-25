using UnityEngine;

namespace Item
{
    public class MeleeDamage : MonoBehaviour
    {
        public float punch=10;
        MeleeGun MG;
        private void Start()
        {
            MG= GetComponentInParent<MeleeGun>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (MG != null && MG.CH.transform == collision.transform) { return; }
            
            Character.Health h;
            h = collision.GetComponentInParent<Character.Health>();
            //if (CH == h) { return; } //Maybe deal damage per frame ? Must save all targets though
            //if (CH == null) { CH = h; }
            if (h != null)
            {
                if (punch != 0)
                {
                    float f = -1;
                    //Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (transform.lossyScale.x < 0) { f = 1; } 
                    h.GetComponent<Rigidbody2D>().AddForce(new Vector2(punch*f,punch), ForceMode2D.Impulse);

                }
                h.DealDamage(1, collision.transform.position, transform.rotation.z);
            }
           // if (FX != null) { FX.DoFX(transform.rotation.eulerAngles.z, transform.position, 10, 3, new int[] { 3 }, 1); }
        }
    }
}
