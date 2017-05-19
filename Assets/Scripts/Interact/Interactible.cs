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

        public float range = 1f; 
        public float cooldown = 1f;
        private float counter;

        private void Awake()
        {
            Menu.GameMaster GM = FindObjectOfType<Menu.GameMaster>();
            Targets=GM.Players;
            if (Targets.Count==0) { Debug.Log("No players?!"); }
            //Targets = new List<Character.Player>(FindObjectsOfType<Character.Player>());
            //Debug.Log("Players found: " + Targets.Count);
        }

        /*
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character.Player pH = collision.GetComponent<Character.Player>();
            if (pH != null)
            {
                if (!Targets.Contains(pH)) { Targets.Add(pH); pH.OnAction += BecomeInteracted; }
            }
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
        */

        public void BecomeInteracted(Transform who)
        {
            if (counter > 0) { return; }
            //Debug.Log("Interacting!");
            if (OnInteract != null) { OnInteract(who); }
            counter = cooldown;
        }

        private void OnDisable()
        {
            //Targets.Clear();
            foreach (Character.Player p in Targets)
            {
                p.OnAction -= PlayerInteract;
            }
        }

        private void OnEnable()
        {
            //if (transform.parent != null) { return; }
            //Targets.Clear();
            foreach (Character.Player p in Targets)
            {
                p.OnAction += PlayerInteract;
            }
        }

        public void PlayerInteract(Transform who)
        {
            //Debug.Log(Vector2.Distance(who.transform.position, transform.position).ToString());
            if (Vector2.Distance(who.transform.position, transform.position) < range) { BecomeInteracted(who); };
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
