using UnityEngine;

namespace Character
{
    public class AISandGolem : MonoBehaviour
    {
        Humus H;
        private Transform Target;
        //private Vector2 LastTarget;

        public float moveforce;
        public float detectmoverange = 2f;
        public float detectpunchrange = 0.3f;
        public Transform movepoint0;
        public Transform movepoint1;
        public float punchforce=5f;
        public bool flip=false;

        private BoxCollider2D punchbox;

        private void Start()
        {
            H = GetComponent<Humus>();
            punchbox = GetComponent<BoxCollider2D>();
            punchbox.enabled = false;
            //TurnAround(!flip);
            if (flip)
            {
                flip = false;
                TurnAround(flip);
                H.Move(-0.1f);
            }
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (Target == null)
            {
                if (collision.tag == "Player")
                {
                    Target = collision.transform;
                    //LastTarget = Target.transform.position;
                }
            } else
            {
                if (collision.tag == "Player")
                {
                    if (collision.transform.position.x-transform.position.x < Target.position.x-transform.position.x)
                    {
                        Target = collision.transform;
                    }
                    
                    //LastTarget = Target.transform.position;
                }
            }

            //Debug.Log(collision.gameObject.name);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (ContactPoint2D cp in collision.contacts)
            {
                if (cp.otherCollider == punchbox) {
                    Vector2 punch;
                    if (!flip) { punch = new Vector2(punchforce,0); }
                    else { punch = new Vector2(-punchforce, 0); }
                    cp.collider.GetComponent<Rigidbody2D>().AddForce(punch);
                }
            }
        }

        private void TurnAround(bool right) {
            if (right && flip)
            {
                punchbox.offset *= -1; moveforce = -moveforce;
                foreach (Transform t in transform)
                {
                    t.localScale *= -1;
                }
                flip = false;
            }
            else if (!right && !flip)
            {
                moveforce = -moveforce; punchbox.offset *= -1;
                foreach (Transform t in transform)
                {
                    t.localScale *= -1;
                }
                flip = true;
            }
        }

        private void MoveForward() {
            H.Move(moveforce);
            if (moveforce == 0) { return; }
            if (H.CurrentAnimation() != 2)
            {
                H.DoAnimation(1, true, false);
            }
        }

        private void Punch() {
            H.DoAnimation(2);
        }

        private void Update()
        {
            if (H.CurrentAnimation()==2 && H.CurrentAnimation(true) == 3)
            {
                punchbox.enabled = true;
            }
            else punchbox.enabled = false;

            if (transform.hasChanged)
            {
                if (H.CurrentAnimation() != 2)
                {
                    H.DoAnimation(1);
                }
                transform.hasChanged = false;
            }

            if (Target != null)
            {
                float distancex = Target.position.x - transform.position.x;
                float distancey = Target.position.y - transform.position.y;

                if (distancex > 0 & moveforce != 0) { TurnAround(true); } else { TurnAround(false); }

                if (Mathf.Abs(distancex) < detectpunchrange) { Punch(); }

                if (Mathf.Abs(distancex ) < detectmoverange ) { MoveForward(); } else { Target = null; }

                if (Mathf.Abs(distancey) > 0.3f && Mathf.Abs(distancex)<0.2f ) { Target = null; }
            }
            //else if(LastTarget != null) { MoveForward(); }
            
        }

    }
}
