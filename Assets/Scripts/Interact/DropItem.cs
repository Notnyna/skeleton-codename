using UnityEngine;

namespace Interact
{
    public class DropItem : MonoBehaviour
    {
        public Transform DropPrefab;
        public int maxdrops;
        private int cdrops;
        public Vector2 dropOffset= new Vector2();

        private void Start()
        {
            Interactible I = GetComponent<Interactible>();
            if (I == null) { Debug.Log("Nothing to interact with! Can be ignored"); }
            else { I.OnInteract += I_OnInteract; }
        }

        private void I_OnInteract(Transform who)
        {
            Drop();
        }

        public void Drop()
        {
            if (DropPrefab == null) { return; }
            if (cdrops < maxdrops)
            {
                Transform drop = Instantiate(DropPrefab);
                drop.position = new Vector3(transform.position.x+dropOffset.x,transform.position.y+dropOffset.y,0);
                cdrops++;
            }
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }

    }
}