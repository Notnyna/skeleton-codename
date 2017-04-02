using UnityEngine;
using System.Collections.Generic;

namespace Character
{
    public class Interactible : MonoBehaviour
    {
        public delegate void WhenActioned(Transform who);
        public event WhenActioned OnInteract;

        public Transform DropPrefab;
        private List<Player> Targets;
        private bool drop;
        public int maxdrops;
        private int cdrops;

        public float cooldown = 10f;
        private float counter;

        public bool animate = false;
        private General.ListAnimation LS;
        public float animations;

        private void Start()
        {
            if (DropPrefab != null) { drop = true; }

            if (animate) { LS = GetComponent<General.ListAnimation>(); }

            Targets = new List<Player>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                Targets.Add(collision.GetComponent<Player>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                Targets.Remove(collision.GetComponent<Player>());
            }
        }

        public void GiveFruit()
        {
            if (cdrops < maxdrops)
            {
                Transform drop = Instantiate(DropPrefab);
                drop.position = transform.position;
                cdrops++;
            }

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
            foreach (Player p in Targets)
            {
                if (counter <= 0 && p.action)
                {
                    if (drop) { GiveFruit(); }
                    if (animate) { Animate(); }
                    if (OnInteract != null) { OnInteract(p.transform); }
                    counter = cooldown;
                }
            }
            if (counter > 0)
            {
                counter -= Time.deltaTime;
            }

        }
    }


}
