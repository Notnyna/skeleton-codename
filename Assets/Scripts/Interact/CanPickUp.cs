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
                if (H.TakeItem(this.transform)) { Owner = H; }
                //Should disable Interactible
            }
        }

        private void OnEnable()
        {
            Character.Humus H = GetComponentInParent<Character.Humus>();
            if (H == null) { Owner = null; return; } //If no parent (owner), can be interacted again. This is very bad?
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
