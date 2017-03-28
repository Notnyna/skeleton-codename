using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class CameraMove : MonoBehaviour
    {
        Camera C;
        float CameraSize;
        public GameObject Target = null;
        public bool mousemove = false;
        Vector2 MovePoint;

        private void Start()
        {
            C = gameObject.GetComponent<Camera>();
            if (C != null) { CameraSize = C.orthographicSize; } else { this.enabled = false; }
        }

        private void Update()
        {
            if (mousemove)
            {
                MovePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MovePoint = MovePoint - new Vector2(transform.position.x, transform.position.y);
                C.orthographicSize = CameraSize + Vector2.Distance(Target.transform.position, transform.position) / 10f;
            }

            if (Target != null)
            {
                MovePoint = new Vector2(Target.transform.position.x, Target.transform.position.y);
            }

            transform.position = new Vector3(MovePoint.x, 0, -10);
        }


    }
}