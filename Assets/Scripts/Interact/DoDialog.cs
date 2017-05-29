using UnityEngine;

namespace Interact
{
    /// <summary>
    /// "xxxx" - write dialogue (x is text) 
    /// pxx - change portrait (x is index)
    /// zxxx - pause (x is float number)
    /// more to come
    /// </summary>
    public class DoDialog : MonoBehaviour
    {
        Menu.StoryModeUI SM;
        public string[] Dialog;
        public bool onetime;
        public Menu.MenuManager MM;

        private void Start()
        {
            Interactible I = GetComponent<Interactible>();
            if (I == null) { Debug.Log("Nothing to interact with! Can be ignored"); }
            else { I.OnInteract += I_OnInteract; }
            SM = MM.GetComponentInChildren<Menu.StoryModeUI>(true); //Cannot find it if its in DONOTDESTROY?
            if (SM == null) { Debug.Log("No story mode window found :("); } //Best to find it in static class, having an actual global time manager would be best.
        }

        private void I_OnInteract(Transform who)
        {
            Character.Player P = who.GetComponent<Character.Player>();
            if (P == null) { return; }  //Only the player can do dialogue.
            MM.ChangeMenu(1); //Hardcoding is not bad
            SM.WriteDialog(Dialog);
            if (onetime) { Destroy(this); }
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }

    }
}