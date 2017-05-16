using UnityEngine;
using System.Collections.Generic;

namespace Interact
{ 
    public class Interactible : MonoBehaviour
    {
        public delegate void WhenActioned(Transform who);
        public event WhenActioned OnInteract;

        private List<Character.Humus> Targets;

        public float cooldown = 1f;
        private float counter;

        public bool animate = false;
        private General.ListAnimation LS;
        public float animations;

        private void Start()
        {
            if (animate) { LS = GetComponent<General.ListAnimation>(); }
            Targets = new List<Character.Humus>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character.Humus cH = collision.GetComponent<Character.Humus>();
            if (cH != null)
            {
                if (Targets.Contains(cH))
                {
                    return;
                }
                Targets.Add(cH);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Character.Humus>() != null)
            {
                Targets.Remove(collision.GetComponent<Character.Humus>());
            }
        }

        private void OnDisable()
        {
            Targets.Clear();
        }

        public void Animate()
        {
            if (animate)
            {
                if (LS.AniIndex > animations) { return; }
                LS.PlayAnimation(LS.AniIndex + 1, true);
            }
        }

        private void Update()
        {
            foreach (Character.Humus p in Targets)
            {
                if (counter < 1 & p.CurrentAnimation()==3) //Maybe change from looking at animations to humus
                {
                    if (OnInteract != null) { OnInteract(p.transform); }
                    if (animate) { Animate(); }
                    counter = cooldown;
                }
            }

            if (counter > 0)  { counter -= Time.deltaTime; }
        }
    }
}
