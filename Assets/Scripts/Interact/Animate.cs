using System;
using UnityEngine;

namespace Interact
{
    public class Animate : MonoBehaviour
    {
        private General.ListAnimation LS;
        public bool increaseani = false;
        public int startani = 0;
        public int endani = 1;
        public bool repeatani = true;

        private void Start()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract += I_OnInteract; } else { Debug.Log("Cant find Interactible!"); }
            LS = GetComponent<General.ListAnimation>();
            if (LS == null) { Debug.Log("Dont forget ListAnimation"); Destroy(this); }
        }

        private void I_OnInteract(Transform who)
        {
            if (repeatani) {
                if (LS.AniIndex == startani) { LS.PlayAnimation(endani,true); } else { LS.PlayAnimation(startani,true); }
                return;
            }
            if (increaseani) {
                LS.PlayAnimation(LS.AniIndex+1);
            }
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }
    }
}
