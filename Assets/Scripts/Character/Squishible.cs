using UnityEngine;

namespace Character
{
    public class Squishible : MonoBehaviour
    {
        public float firmness=2f;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") | collision.gameObject.CompareTag("Monster"))
            {
                firmness -= Time.deltaTime;
                if (firmness < 0) { Squish(); }
            }
        }

        void Squish()
        {
            Destroy(gameObject);
        }
    }
}
