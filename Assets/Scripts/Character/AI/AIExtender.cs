using UnityEngine;

namespace Character.AI
{
    public class AIExtender : MonoBehaviour,AIFunc
    {
        private AIBase AIB;
        private General.Extender EX;
        public bool extendOnEnemy = true;
        public float extendDist = 10f;
        public int attackex=20;
        public bool jumpex=false;

        public void ConditionCheck()
        {
            if (AIB != null && AIB.hostile) {
                bool target = AIB.NoTarget();
                if (!target)
                {
                    float d = Mathf.Abs(AIB.CalculateTarget());
                    if ( d< extendDist) {
                        if (jumpex && d < 4) { EX.ExtendPercent(100); }
                        else EX.ExtendPercent(attackex); }
                    else { EX.ExtendPercent(0); }
                }
            }
        }

        public void Eject()
        {
            AIB = null;
            EX.extensionEnd = null;
            Destroy(EX);
            Destroy(this);
        }

        public bool Execute()
        {
            ExtendRandom();
            //ExtendRandom();
            return true;
        }

        public void HalfPower()
        {
            return;
            //throw new NotImplementedException();
        }

        //private void Extend(int p) { EX.ExtendPercent(p); }

        private void ExtendRandom()
        {
            //if(AIB!=null && AIB.NoTarget())
            if (!action)
            {
                EX.ExtendPercent(Random.Range(0,100));
                count0 = extendtime;
                action = true;
            }
            //else { EX.ExtendPercent(Random.Range(0,100)); }
           
        }
        bool action;
        float count0;
        public float extendtime=0.5f;

        private void Update()
        {
            if (count0 > 0) { count0 -= Time.deltaTime; }
            else action = false;
        }

        public void SetAIB(AIBase aib)
        {
            AIB = aib;
            EX = GetComponentInChildren<General.Extender>();
            if (EX == null) { Debug.Log("No extender!"); }
            //extendtime = (EX.maxext+ 0.02f)* EX.cooldown;
            //throw new NotImplementedException();
        }
    }
}
