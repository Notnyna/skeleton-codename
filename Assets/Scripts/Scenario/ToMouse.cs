using UnityEngine;

namespace Scenario
{
    public class ToMouse : MonoBehaviour
    {
        public Transform MouseTarget;

        private void OnTriggerStay2D(Collider2D collision)
        {
            MouseTarget = collision.transform;
        }

        private void Update()
        {
            transform.position = -Menu.UsefulStuff.MouseToPointVector(Vector2.zero);// Camera.main.ScreenPointToRay(Input.mousePosition).direction*40;
        }

    }
}
