using UnityEngine;
using UnityEngine.UI;

namespace Scenario
{
    public class SendToStoryText: MonoBehaviour
    {
        public string Line = "Something occured";
        public float clickArea = 0.1f;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if (new Vector2(MousePos.x-transform.position.x, MousePos.y-transform.position.y).magnitude < clickArea)
                {
                    SendToBox();
                }
            }
        }
        void SendToBox() {
            GameObject[] M = GameObject.FindGameObjectsWithTag("Menu");
            foreach (GameObject Menu in M)
            {
                if (Menu.name=="StoryModeText")
                {
                    Menu.GetComponentInChildren<Text>().text += "\n"+ Line;
                }
            }

        }
    }
}
