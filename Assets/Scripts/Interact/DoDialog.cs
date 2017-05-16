using UnityEngine;

namespace Interact
{
    public class DoDialog : MonoBehaviour
    {
        Menu.StoryModeUI SM;
        public string[] Dialog;

        private void Start()
        {
            Interactible I = GetComponent<Interactible>();
            if (I == null) { Debug.Log("Nothing to interact with! Can be ignored"); }
            else { I.OnInteract += I_OnInteract; }
            SM = GetComponentInParent<Menu.StoryModeUI>();
            if (SM == null) { Debug.Log("No story mode window found :("); }
        }

        private void I_OnInteract(Transform who)
        {
            Menu.MenuManager MM = GetComponentInParent<Menu.MenuManager>();
            if (MM != null) { MM.ChangeMenu(1); }
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }

    }
}