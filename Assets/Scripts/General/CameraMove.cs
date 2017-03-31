using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class CameraMove : MonoBehaviour
    {
        Camera C;
        float CameraSize;

        public Transform Target;
        private Character.Player[] Players;
        private int cPlayer;

        public bool mousemove;
        public string switchbutton = "q";
        public float cameraspeed = 0.1f;
        public float focusspeed = 1f;

        public bool focused;

        Vector2 MovePoint;

        private void Start()
        {
            C = gameObject.GetComponent<Camera>();
            if (C != null) { CameraSize = C.orthographicSize; } else { this.enabled = false; }
            Players = FindObjectsOfType<Character.Player>();
            SwitchToPlayer(3);
        }

        private void FocusCamera(bool focus)
        {
            bool doSwitch=false;

            if (!focused & !focus) { doSwitch = false; }
            if (focused & !focus) { doSwitch = true; }
            if (focused & focus) { doSwitch = false; }
            if (!focused & focus) { doSwitch = true; }

            if (doSwitch)
            {
                focused = !focused;
                float temp = cameraspeed;
                cameraspeed = focusspeed;
                focusspeed = temp;
            }
        }

        public void SwitchToPlayer(int p)
        {
            Players[cPlayer].enabled = false;
            cPlayer = p;
            if (cPlayer >= Players.Length) { cPlayer = 0; }
            Target = Players[cPlayer].transform;
            Players[cPlayer].enabled = true;

            FocusCamera(false);
        }

        private void Update()
        {
            if (mousemove)
            {
                MovePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MovePoint = MovePoint - new Vector2(transform.position.x, transform.position.y);
                C.orthographicSize = CameraSize + Vector2.Distance(Target.transform.position, transform.position) / 10f;
            }

            if (Input.GetKeyDown(switchbutton))
            {
                SwitchToPlayer(cPlayer+1);
            }

            if (Target != null)
            {
                if (!focused & Mathf.Abs((Target.position.x - transform.position.x)) < 0.05f)
                {
                    FocusCamera(true);
                }

                MovePoint = new Vector2(Target.transform.position.x, 0); // Target.transform.position.y);

                MovePoint.x = Mathf.Lerp(transform.position.x,MovePoint.x,cameraspeed);
            }


            transform.position = new Vector3(MovePoint.x, 0, -10);
        }


    }
}