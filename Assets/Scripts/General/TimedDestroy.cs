using UnityEngine;

namespace General
{
    public class TimedDestroy : MonoBehaviour
    {
        public float timer=1f;

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0) { Destroy(gameObject); }
        }
    }
}
