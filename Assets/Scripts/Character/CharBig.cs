using UnityEngine;

namespace Character
{
    public class CharBig :MonoBehaviour
    {
        CircleCollider2D punchbox;
        Humus H;
        public float punchforce =100f;
        public float monsterpunchmult = 2f;

        private void Start()
        {
            H = GetComponent<Humus>();
            punchbox = GetComponent<CircleCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (ContactPoint2D cp in collision.contacts)
            {
                if (cp.otherCollider == punchbox)
                {
                    Vector2 punch;
                    if ( !H.flip ) { punch = new Vector2(punchforce, 0); }
                    else { punch = new Vector2(-punchforce, 0); }
                    

                    if (collision.gameObject.CompareTag("Monster"))
                    {
                        cp.collider.GetComponent<Rigidbody2D>().AddForce(punch*monsterpunchmult);
                    }
                    else {
                        cp.collider.GetComponent<Rigidbody2D>().AddForce(punch);
                    }
                }
            }
        }

        private void Flip(bool flip) {
            if (flip)
            {
                punchbox.offset = new Vector2(-0.08f,0) ;
            }
            else
            {
                punchbox.offset = new Vector2(0.08f, 0);
            }
        }

        private void Update()
        {
            if (transform.hasChanged) {
                Flip(H.flip);
                transform.hasChanged = false;
            }

            if (H.CurrentAnimation() == 2 )
            {
                punchbox.enabled = true;
            }
            else punchbox.enabled = false;
        }


    }
}
