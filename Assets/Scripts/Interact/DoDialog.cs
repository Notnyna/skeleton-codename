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

        private void Awake()
        {
            Interactible I = GetComponent<Interactible>();
            if (I == null) { Debug.Log("Nothing to interact with! Can be ignored"); }
            else { I.OnInteract += I_OnInteract; }
            SM = FindObjectOfType<Menu.StoryModeUI>();
            if (SM == null) { Debug.Log("No story mode window found :("); } //Best to find it in static class, having an actual global time manager would be best.
        }

        private void I_OnInteract(Transform who)
        {
            Character.Player P = who.GetComponent<Character.Player>();
            if (P == null) { return; }  //Only the player can do dialogue.
            Menu.MenuManager MM = FindObjectOfType<Menu.MenuManager>();
            MM.ChangeMenu(1); //Hardcoding is not bad
            SM.WriteDialog(Dialog); 
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }

    }
}