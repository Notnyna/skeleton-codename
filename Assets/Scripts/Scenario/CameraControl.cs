using UnityEngine;

namespace Scenario
{
    public class CameraControl : MonoBehaviour
    {
        public delegate void WhenTargetSwitch(Transform T);
        public event WhenTargetSwitch TargetSwitched;

        public Transform Target;

        public bool mousemove;
        public float cameraspeed = 0.1f;
        public float focusspeed = 1f;
        public Vector2 Offset = new Vector2(0,0);

        public bool focused;
        Vector2 MovePoint;

        private void FocusCamera(bool focus)
        {
            bool doSwitch = false;

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
            //To account for some engine related delays
            if (TargetSwitched != null) { TargetSwitched(Target); }
        }

        public void SwitchTarget(Transform t)
        {
            Target = t;
            FocusCamera(false);
            if (TargetSwitched!=null) { TargetSwitched(t); }
        }

        private void Update()
        {
            if (Target != null)
            {
                if (!focused & Mathf.Abs((Target.position.x - transform.position.x)) < 0.1f)
                {
                    FocusCamera(true);
                }

                MovePoint = new Vector2(Target.transform.position.x, Target.transform.position.y)+Offset; // Target.transform.position.y);

                MovePoint.x = Mathf.Lerp(transform.position.x, MovePoint.x, cameraspeed);
                MovePoint.y = Mathf.Lerp(transform.position.y, MovePoint.y, cameraspeed);
            }

            if (mousemove)
            {
                Vector2 Mpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MovePoint += new Vector2(Mpoint.x - transform.position.x, Mpoint.y - transform.position.y);
                //C.orthographicSize = CameraSize + Vector2.Distance(Target.transform.position, transform.position) / 10f;
            }

            transform.position = new Vector3(MovePoint.x, MovePoint.y,transform.position.z);
        } //EndUpdate
    }
}
