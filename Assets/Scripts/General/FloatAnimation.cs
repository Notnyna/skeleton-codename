using System;
using UnityEngine;

namespace General
{
    class FloatAnimation : MonoBehaviour
    {
        // Might be problematic interpolating with rigidbodies
        public float deltafloatx = 0.001f;
        public float deltafloaty = 0.001f;
        public float speed = 0.01f;
        public bool fry = false;
        public bool frx = false;
        Vector2 D;
        Vector2 P;

        private void Start()
        {
            P = new Vector2(transform.localPosition.x, transform.localPosition.y);
            D = new Vector2();
        }


        public void RandomizeDirection()
        {
            //if (Vector2.Distance(D,P) > deltafloatx+deltafloaty)
            D = new Vector2();
            float r = UnityEngine.Random.Range(-100, 100);
            if (!frx) { D.x += (deltafloatx / 100) * r; }

            r = UnityEngine.Random.Range(-100, 100);
            if (!fry) { D.y += (deltafloaty / 100) * r; }
            //gameObject.transform.localPosition = D;

        }

        float timer = 1f;
        void Update()
        {
            //Debug.LogFormat("{0}",((D+P)*100f).ToString());
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                RandomizeDirection();
                timer = 0.1f;
            }
            gameObject.transform.localPosition = D + P;

        }

    }
}

