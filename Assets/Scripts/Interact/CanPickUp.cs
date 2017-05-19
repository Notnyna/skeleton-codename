using UnityEngine;

namespace Interact
{
    public class CanPickUp : MonoBehaviour
    {
        public Character.Humus Owner;
        //private bool interactflag = false;

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
                Owner = H; H.TakeItem(this.transform);
                //Should disable Interactible
            }
        }

        /*private void OnDisable()
        {
            OnDestroy();
        }*/

        private void OnEnable()
        {
            Character.Humus H = GetComponentInParent<Character.Humus>();
            if (H == null) { Owner = null; return; } //If no parent (owner), can be interacted again.
            if (H != Owner) { H = Owner; } //If different parent (for whatever reason), he is now the owner!
            //If enabled and still belongs to owner, do nothing. (is still not interactible)
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
