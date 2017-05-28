using UnityEngine;

namespace Character.AI
{
    /// <summary>
    /// Gets and updates Target info on trigger collisions.
    /// Bullets too. Optionially.
    /// The head can be talked to when the body is dead, usually.
    /// </summary>
    public class AIEyes : MonoBehaviour, AIFunc
    {
        private AIBase AIB;
        private Transform See;
        private float count;
        public float blindtime=6f;

        public void SetAIB(AIBase aib)
        {
            AIB = aib;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) { return; }
            //if (collision.GetComponent<Health>().GetPercentHP() < 20) { See = null; return; }
            if (See == collision.transform) { count = blindtime; return; }
            See = collision.transform;
            count = blindtime;
           
        }


        public void ConditionCheck()
        {
            if (AIB != null) { AIB.SwitchTargets(See, true); }
        }

        private void Update()
        {
            if (See != null) {
                if (count > 0) { count -= Time.deltaTime; } else { See = null; }
            }
        }

        public bool Execute()
        {
            return false; //Eyes cannot do anything really
        }

        public void Eject()
        {
            if (!AIB.hostile) { AIB.hostile = true; }
            AIB.SwitchTargets(null,false);
            AIB = null;
        }

        public void HalfPower()
        {
            if (blindtime > 1f) { blindtime *= 0.5f; }
        }
    }
}
