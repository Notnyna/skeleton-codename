using System;
using UnityEngine;

namespace Character
{
    public class AIBase : MonoBehaviour
    {
        Humus H;
        Rigidbody2D RB;
        Transform Target;
        public bool flip = false;
        public float moveforce;
        public float detectA1;
        public float detectA2;

        private void Start()
        {
            H = GetComponent<Humus>();
            RB = GetComponent<Rigidbody2D>();
            if (H | RB == null) { Debug.Log("Missing components, AI might not function properly in "+gameObject.name); }
            if (flip) {
                flip = false;
                TurnAround(flip);
                H.Move(-0.1f);
            }
        }

        private void MoveForward()
        {
            if (moveforce == 0) { return; }
            if (H.Move(moveforce))
            {
                H.DoAnimation(1, true, false);
            }
        }

        private void TurnAround(bool right)
        {
            /*
            if (right && flip)
            {
                //punchbox.offset *= -1;
                moveforce = -moveforce;
                foreach (Transform t in transform)
                {
                    t.localScale *= -1;
                }
                flip = false;
            }
            else if (!right && !flip)
            {
                // punchbox.offset *= -1;
                moveforce = -moveforce;
                foreach (Transform t in transform)
                {
                    t.localScale *= -1;
                }
                flip = true;
            }
            */
        }

        private void CalculateTarget()
        {
            if (Target == null) { return; }
            float distancex = Target.position.x - transform.position.x;
            float distancey = Target.position.y - transform.position.y;

            if (distancex > 0 & moveforce != 0) { TurnAround(true); } else { TurnAround(false); }

            if (Mathf.Abs(distancex) < detectA2) {  }

            if (Mathf.Abs(distancex) < detectA1) {  } //else { Target = null; }

            if (Mathf.Abs(distancey) > 0.3f && Mathf.Abs(distancex) < 0.2f) { Target = null; }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag != "Player") { return; }
            if (Target == null)
            {
                Target = collision.transform;
            } // If new target is closer, switch targets
            else if (Mathf.Abs(collision.transform.position.x - transform.position.x) < Mathf.Abs(Target.position.x - transform.position.x))
                {
                    Target = collision.transform;
                }
        }

        private void Update()
        {
            CalculateTarget();
        }

    }
}
