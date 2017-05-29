using UnityEngine;

namespace Scenario
{
    public class EventZone : MonoBehaviour
    {
        public Menu.GameMaster GM;
        public Transform Ctarget;
        public float time = 1f;
        int howmany=1;
        public Transform Etarget;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (howmany == 0) { return; }
            //should also check if it is enabled first
            if (collision.CompareTag("Player") ) {
                if (Ctarget != null) { GM.DirectCamera(time,Ctarget); }
                if (Etarget != null) {
                    Etarget.gameObject.SetActive(true);
                    Transform[] T = new Transform[Etarget.childCount];
                    for (int i = 0; i < Etarget.childCount; i++)
                    {
                        T[i] = Etarget.GetChild(i);
                    }
                    for (int i = 0; i < T.Length; i++)
                    {
                        T[i].parent = null;
                    }
                    Destroy(Etarget.gameObject);
                }
                howmany--;
            }
        }

    }
}
