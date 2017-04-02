using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Scenario
{
    public class LoadScene : MonoBehaviour
    {
        Character.Interactible I;
        public int loadsceneid = 0;
        public Vector2 Offset = new Vector2(0,0);

        private List<Transform> Transfer;

        private void Start()
        {
            I = GetComponent<Character.Interactible>();
            if (I == null | !this.enabled) { Destroy(gameObject); }
            I.OnInteract += I_OnInteract;
            Transfer = new List<Transform>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Transfer.Contains(collision.transform))
            {
                Transfer.Add(collision.transform);
            }
           
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Transfer.Remove(collision.transform);
        }

        private void PositionWhoToTransfer()
        {
            foreach (Transform t in Transfer)
            {
                //Debug.Log(Vector3.MoveTowards(t.position, transform.position, 0).ToString() + "  " + t.name);
                t.position = new Vector3(
                    Offset.x + Mathf.Abs(t.transform.position.x)-Mathf.Abs(transform.position.x),
                    Offset.y + Mathf.Abs(t.transform.position.y) - Mathf.Abs(transform.position.y)
                    );
                DontDestroyOnLoad(t);
            }
        }

        private void SavePlayerInfo(Transform P) { }

        private void I_OnInteract(Transform who)
        {
            Color col = new Color(0, 0, 0, 50);
            GetComponent<Renderer>().material.color = col;
            PositionWhoToTransfer();


            SceneManager.LoadScene(loadsceneid);
        }

        private void OnGUI()
        {

        }
    }
}
