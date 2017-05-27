using System;
using UnityEngine;

namespace Character.AI
{
    public class AILegs : MonoBehaviour, AIFunc
    {
        //Maybe add something here like knockback stabilization or something.
        public AIBase AIB;
        public void ConditionCheck()
        {
            return;
        }

        public void Eject()
        {
            AIB = null;
        }

        public bool Execute()
        {
            return false;
        }

        public void SetAIB(AIBase aib)
        {
            AIB = aib;
        }
    }
}
