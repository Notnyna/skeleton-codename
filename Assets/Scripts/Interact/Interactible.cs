using UnityEngine;
using System.Collections.Generic;

namespace Interact
{ 
    /// <summary>
    /// All interaction effects use this (event). 
    /// All effects should possibly unsubscrive -=event on disabling? Must test to make sure.
    /// How to handle multiple interactibles close together?
    /// Now I realize this is the wrong way about it.
    /// </summary>
    public class Interactible : MonoBehaviour
    {
        public delegate void WhenActioned(Transform who);
        public event WhenActioned OnInteract;

        private List<Character.Player> Targets;

        public float cooldown = 1f;
        private float counter;

        public bool animate = false;
        private General.ListAnimation LS;
        public int playanimation;
        public int increaseani=0;

        private void Start()
        {
            if (animate) { LS = GetComponent<General.ListAnimation>(); if (LS == null) { animate = false; } }
            Targets = new List<Character.Player>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character.Player pH = collision.GetComponent<Character.Player>();
            if (pH != null)
            {
                if (!Targets.Contains(pH)) { Targets.Add(pH); pH.OnAction += BecomeInteracted; }
            }
        }

        public void BecomeInteracted(Transform who)
        {
            if (counter > 0) { return; } 
            if (OnInteract != null) { OnInteract(who); }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Character.Player pH = collision.GetComponent<Character.Player>();
            if (pH != null)
            {
                Targets.Remove(pH);
                pH.OnAction -= BecomeInteracted;
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
                LS.PlayAnimation(playanimation+increaseani, true);
            }
        }

        private void Update()
        {
            if (counter > 0) { counter -= Time.deltaTime; }
            /*else
            {
                //if (OnInteract != null) { OnInteract(p.transform); }
                if (animate) { Animate(); }
                counter = cooldown;
            }*/
        }
    }
}
