using System.Collections.Generic;
using UnityEngine;

namespace Character.AI
{
    /// <summary>
    /// The AI base serves as the command module of the AI (rob0t).
    /// It only manages the AI functions allowing them to execute and giving a target;
    /// Also maintains every childs health components.
    /// Becomes hostile after being attacked, or is evil and detects player. (depends on head)
    /// </summary>
    public class AIBase : MonoBehaviour
    {
        Humus H;
        Rigidbody2D RB;
        Transform Target; //Only one target per AIbase
        public bool flip = false;
        public float moveforce;
        public float jumpforce; 
        public bool hostile;
        List<AIFunc> AIF; //List of ai functions (in children only for now!)

        General.SpawnFX SX;

        private void Start()
        {
            AIF = new List<AIFunc>(GetComponentsInChildren<AIFunc>()); // Module list
            SX = GetComponent<General.SpawnFX>();
            H = GetComponent<Humus>();
            RB = GetComponent<Rigidbody2D>();
            if (flip) {
                H.Move(0.1f);
            }
            foreach (AIFunc aif in AIF)
            {
                aif.SetAIB(this);
            }
        }

        public void Move(float mult)
        {
            if (moveforce == 0) { return; }
            if (H.Move(moveforce*mult))
            {
                H.DoAnimation(1);
            }
        }

        public float CalculateTarget()
        {
            if (Target == null) { return 0; }
            float distancex = Target.position.x - transform.position.x;

            //float distancey = Target.position.y - transform.position.y;

            //if (Mathf.Abs(distancex) < detectA2) {  }
            //if (Mathf.Abs(distancex) < detectA1) {  } //else { Target = null; }

            //if (Mathf.Abs(distancey) > 0.3f && Mathf.Abs(distancex) < 0.2f) { Target = null; }
            return distancex;
        }

        public void Eject(Transform child)
        {
            foreach (Transform c in transform)
            {
                if (child == c)
                {
                    if (c is AIFunc)
                    {
                        c.GetComponent<AIFunc>().Eject();
                        AIF.Remove(c.GetComponent<AIFunc>());
                    }
                    Ejection(c);
                }
            }
        }

        private void Ejection(Transform c) //Launch and propel the module out of existance. (Don't actually) (Ok maybe just a little)
        {
            Rigidbody2D crb = c.GetComponent<Rigidbody2D>();
            c.gameObject.layer = 8; //To debrisFX layer
            c.parent = null;
            if (SX != null) {
                
            }
            //Would be nice to move each component from the explosion.
            //Too lazy to do everything by physics joints (and so I wont)
        }

        public void SwitchTargets(Transform t, bool dc)
        {
            if (Target == null)
            {
                Target = t;
                return;
            }
            if (Target == t) { return; }
            if (t == null) { Target = null; return; }
            // If new target is closer, switch targets
            if (dc)
            {
                if (Mathf.Abs(Target.position.x - transform.position.x) > Mathf.Abs(t.position.x - transform.position.x))
                {
                    Target = t;
                }
            }
        }

        public bool NoTarget() { if (Target == null) { return true; } else return false; }

        private void Update()
        {
            //For use with Spawning FX, not much else.
        }

        private void LateUpdate()
        {
            //float dist = CalculateTarget();
            foreach (AIFunc aif in AIF)
            {
                aif.ConditionCheck();
            }
        }

    }
}
