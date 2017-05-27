using UnityEngine;
using System.Collections.Generic;

namespace Character.AI
{
    public class AIPatrol : MonoBehaviour, AIFunc
    {
        AIBase AIB;
        public float btinterv = 5f; //Time between breaks
        private float breaktime; //BREAKTIME
        private bool movebreak; // True-CanMove false-CannotMove
        private float mm; // move mult
        public bool patrol;
        List<AILegs> LG;
        private int dlegs; //default amount of legs

        public void SetAIB(AIBase aib)
        {
            AIB = aib;
            LG = new List<AILegs>(aib.GetComponentsInChildren<AILegs>());
            dlegs = LG.Count;
        }

        public void ConditionCheck()
        {
            if (AIB == null) { return; }
            if (AIB.hostile)
            {
                if (AIB.NoTarget()) {
                    //Patrol around angrily with short breaks if no Target
                    MoveRandom(btinterv/2);
                    return;
                } else MoveTarget();
                //If Target is around, go towards him.
            }
            else {
                //Stay in place 
                if (patrol) { MoveRandom(btinterv); }
            }
        }

        private void MoveRandom(float time)
        {
            if (breaktime > 0) { return; }
            if (!movebreak) { movebreak = true; breaktime = time; return; }
            mm = Random.Range(0,3);
            float d = Random.Range(0,2);
            if (d == 0 ) { d = -1; }
            mm = d*(1/(float)dlegs)*LG.Count - mm/10;
            AIB.Move(mm);
            breaktime = time;
            movebreak = !movebreak;
        }

        private void MoveTarget()
        {
            if (!movebreak) {  movebreak = true; }
            if (AIB == null) { return; }
            float d = AIB.CalculateTarget();
            if (d > 0) { mm = (1 / (float)dlegs) * LG.Count; } else { mm = -(1 / (float)dlegs) * LG.Count; }
        }

        public bool Execute()
        {
            return false; //Ie is not executable
        }

        public void Eject()
        {
            AIB = null;
        }

        private void Update()
        {
            if (breaktime > 0) { breaktime -= Time.deltaTime; }
            if (movebreak) { if (AIB != null) { AIB.Move(mm); } }
        }

        private void LateUpdate()
        {
            if (AIB != null && movebreak)
            {
                foreach (AILegs l in LG)
                {
                    if (l.AIB == null) { LG.Remove(l); return; }
                }   
            }
        }
    }
}