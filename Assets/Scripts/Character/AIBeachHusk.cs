using UnityEngine;

namespace Character
{
    public class AIBeachHusk: MonoBehaviour
    {
        Humus H;

        private void Start()
        {
            H = GetComponent<Humus>();
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
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && collision.transform.position.y > transform.position.y+0.1f)
            {
                H.DoAnimation(2, false,true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                H.DoAnimation(1, true, false);
            }
        }


    }
}
