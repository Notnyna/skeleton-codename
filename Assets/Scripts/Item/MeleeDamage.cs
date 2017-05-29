using UnityEngine;

namespace Item
{
    public class MeleeDamage : MonoBehaviour
    {
        public float punch=10;
        public int damage=1;
        public bool knockbackowner = true;
        MeleeGun MG;
        General.SpawnFX FX;

        private void Start()
        {
            MG = GetComponentInParent<MeleeGun>();
            FX = GetComponent<General.SpawnFX>();
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
                //Direction should always be away from the weapon
                bool f = false;
                if (transform.lossyScale.x > 0) { f = true; } 
                Vector2 dirup = Menu.UsefulStuff.FromRotationToVector(transform.rotation.eulerAngles.z, f); //only for up vector?

                Vector2 direction = new Vector2(transform.position.x- collision.transform.position.x,dirup.y*5);
                h.DealDamage(damage, collision.transform.position, -direction, punch);

                if (knockbackowner)
                {
                    if (MG != null && MG.CH != null)
                    {
                        Rigidbody2D pRB = MG.CH.GetComponent<Rigidbody2D>();
                        if (pRB != null)
                        {
                            pRB.AddForce(new Vector2((punch * -1) / 2, 0), ForceMode2D.Impulse);
                        }
                    }
                }
                if (FX != null) { FX.DoFX(direction,transform.position,50,5,new int[] {0,1},2); }
            }
        }
    }
}
