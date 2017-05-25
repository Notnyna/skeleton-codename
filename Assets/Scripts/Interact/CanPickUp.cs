using UnityEngine;

namespace Interact
{
    public class CanPickUp : MonoBehaviour
    {
        public Character.Humus Owner;
        //private bool interactflag = false;
        public Vector2 HoldOver = new Vector2();

        private void Start()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract += I_OnInteract; }
        }

        private void I_OnInteract(Transform who)
        {
            Character.Humus H = who.GetComponent<Character.Humus>();
            if (H==null) { return; }
            GiveItself(H);
        }

        public void GiveItself(Character.Humus H)
        {
            //Debug.Log("Giving if possible!");
            if (Owner == null && Owner!=H) {
                if (H.TakeItem(this.transform)) {
                    Owner = H;
                    Collider2D[] C = GetComponents<Collider2D>();
                    for (int i = 0; i < C.Length; i++)
                    {
                        C[i].isTrigger = true;
                    }
                }
                //Should disable Interactible and collision
            }
        }

        private void OnEnable()
        {
            Character.Humus H = GetComponentInParent<Character.Humus>(); // Just how far does it detect?
            //Debug.Log(H.gameObject.name);
            if (H !=null && H != Owner) { H = Owner; return; } //If different parent (for whatever reason), he is now the owner!
            if (H == Owner) { return; }            //If enabled and still belongs to owner, do nothing. (is still not interactible)
            if (H == null) { Owner = null; } //If no parent (owner), can be interacted again. This is very bad?
            Collider2D[] C = GetComponents<Collider2D>();
            for (int i = 0; i < C.Length; i++)
            {
                C[i].isTrigger = false;
            }

        }

        private void OnTransformParentChanged()
        {
            OnEnable();
        }
        
        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }
        
    }
}
