using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class DragMouse : MonoBehaviour
    {

        [Range(100f, 1000f)]
        public float speed = 100f;
        Rigidbody2D rb;
        public float neardistance = 1f;
        public bool fixedmove = false;
        public GameObject Target = null;

        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        void DragToMouse()
        {
            //Vector2 MPos = Input.mousePosition;
            Vector2 FDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.LogFormat("{0}", X);
            if (Vector2.Distance(transform.position, FDir) > neardistance)
            {
                FDir = FDir - new Vector2(transform.position.x, transform.position.y);
                FDir = FDir.normalized * speed;
                rb.AddForce(FDir);
            }
        }

        void MoveToMouse()
        {
            Vector2 FDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FDir = FDir - new Vector2(transform.position.x, transform.position.y);
            //Not done
            // if (Vector2.Distance(Target.transform.position, FDir) > neardistance) { rb.MovePosition(FDir); }
        }

        private void Update()
        {
            if (fixedmove) { MoveToMouse(); }
        }

        void FixedUpdate()
        {
            if (!fixedmove) { DragToMouse(); }
        }
    }
}