using UnityEngine;

namespace Character
{
    public class AIBeachHusk: MonoBehaviour
    {
        Humus H;
        Rigidbody2D RB;
        bool squished;
        Player P;
        float savedjumpf;
        public float jumpmult=1.8f;

        private void Start()
        {
            H = GetComponent<Humus>();
            RB = GetComponent<Rigidbody2D>();
            //transform.hasChanged = false;
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                if (H.CurrentAnimation() != 2)
                {
                    H.DoAnimation(1);
                }
                transform.hasChanged = false;
            }
            if (RB.velocity.magnitude > 3f)
            {
                RB.freezeRotation = false;
            }
            if (squished)
            {
                if (P != null)
                {
                    if (savedjumpf == 0) { savedjumpf = P.jumpforce; }
                    P.jumpforce = savedjumpf * jumpmult;
                }
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.transform.position.y > transform.position.y+0.1f)
            {
                H.DoAnimation(2, false,true);
                squished = true;
                P = collision.GetComponent<Player>();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

            H.DoAnimation(1, true, false);
            if (P != null)
            {
                P.jumpforce = savedjumpf;
                savedjumpf = 0;
                P = null;
            }
            squished = false;
        }


    }
}
