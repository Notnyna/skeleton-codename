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
                if (LS.AniIndex == endani | who==transform) {
                    LS.PlayAnimation(startani,true);
                    foreach (Transform c in transform)
                    {
                        c.gameObject.SetActive(false);
                    }
                } else {
                    LS.PlayAnimation(endani, true);
                    foreach (Transform c in transform)
                    {
                        c.gameObject.SetActive(true);
                    }
                }
                return;
            }
            if (increaseani) {
                LS.PlayAnimation(LS.AniIndex+1);
            }
            foreach (Transform c in transform)
            {
                c.gameObject.SetActive(true);
            }
        }

        float count;
        public float autoani;
        private void Update()
        {
            if (count > 0) { count -= Time.deltaTime; } else { if (autoani > 0) { count = autoani; I_OnInteract(transform); } }
        }

        private void OnDestroy()
        {
            Interactible I = GetComponent<Interactible>();
            if (I != null) { I.OnInteract -= I_OnInteract; }
        }
    }
}
