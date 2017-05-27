﻿using System;
using UnityEngine;

namespace Character.AI
{
    /// <summary>
    /// When Target is close enough, attack!
    /// Either gun or meleeweapon (must be a child component!)
    /// </summary>
    public class AIAttack : MonoBehaviour, AIFunc
    {
        private AIBase AIB;
        private Item.MeleeGun MG;
        private Item.Gun G;
        public float attackdistance=10f;

        public void SetAIB(AIBase aib)
        {
            AIB = aib;
        }

        private void Start()
        {
            MG = GetComponentInChildren<Item.MeleeGun>();
            if (MG == null) { G= GetComponentInChildren<Item.Gun>(); }
        }

        public void ConditionCheck()
        {
            if (AIB == null) { return; }
            if (!AIB.hostile) { return; }

            float d = AIB.CalculateTarget();
            if (d == 0) { return; } //Is hostile but no target in sight.

            if (Mathf.Abs(d) < Mathf.Abs(attackdistance))
            {
                Execute();
            }
        }

        public bool Execute()
        {
            if (MG != null) { MG.Fire(); }
            else if (G != null) { G.Fire(); }
            //Debug.Log("Shooting!");
            return true;
        }

        public void Eject()
        {
            AIB = null;
        }
    }
}
