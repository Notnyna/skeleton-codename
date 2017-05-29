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
        //Rigidbody2D RB;
        Transform Target; //Only one target per AIbase
        public bool flip = false;
        public float moveforce;
        public float jumpforce; 
        public bool hostile;
        List<AIFunc> AIF; //List of ai functions (in children only for now!)
        Health HP;
        //General.SpawnFX SX;
        bool insane;

        private void Awake()
        {
            AIF = new List<AIFunc>(GetComponentsInChildren<AIFunc>()); // Module list
            //SX = GetComponent<General.SpawnFX>();
            H = GetComponent<Humus>();
            //RB = GetComponent<Rigidbody2D>();
            HP = GetComponent<Health>();
            if (!HP.Global) { Debug.Log("AIBase uses a global health!"); }
            HP.HpChanged += MainHpChanged;
            HP.OnDeath += KillAI;
            HP.OnPartDeath += OnPartDeath;
            //HP.OnBleedout += OnPartBleed; //Maybe also implement OnPartHeal?!

            if (flip) {
                H.Move(0.1f);
            }
            foreach (AIFunc aif in AIF)
            {
                aif.SetAIB(this);
            }
        }

        private void MainHpChanged(Health who)
        {
            if (who.GetPercentHP() > 99) { return; }
            hostile = true;
            if (Target != null) { return; }
            foreach (AIFunc aif in AIF)
            {
                aif.Execute();
            }
        }

        private void OnPartBleed(Transform who)
        {
            AIFunc w =who.GetComponent<AIFunc>();
            if (w != null) { w.HalfPower(); }
        }

        private void OnPartDeath(Transform who)
        {
            Eject(who);
            if (who.GetComponent<AIEyes>()) { insane = true; }
        }

        private void KillAI(Health who)
        {
            HP.OnDeath -= KillAI;
            HP.OnPartDeath -= OnPartDeath;
            Eject(null,true);
        }

        public void Move(float mult)
        {
            if (moveforce == 0) { return; }
            //Debug.Log(mult);
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

        public void Eject(Transform child, bool death=false) // On death eject all modules. Destroy this gameObject
        {
            foreach (Transform c in GetComponentsInChildren<Transform>()) //Maybe useless check?!
            {
                if (child == c | death)
                {
                    AIFunc cai = c.GetComponent<AIFunc>();
                    if (cai != null) { cai.Eject(); AIF.Remove(cai);  }
                    Ejection(c);
                    if (!death) { break; }
                }
            }
            if (death) { this.death=death; H.DropAll(); }
        }

        private void Ejection(Transform c) //Launch and propel the module out of existance. (Don't actually) (Ok maybe just a little)
        {
            if (c.CompareTag("Monster")==false) { return; }
            Rigidbody2D crb = c.GetComponent<Rigidbody2D>();
            General.MoveAnimation cMV = c.GetComponent<General.MoveAnimation>();
            if (cMV != null) { Destroy(cMV); }
            //if (Random.Range(0, 2) == 0) { c.gameObject.layer = 8; }//To debrisFX layer
            c.parent = null;
            if(crb ==null) { crb=c.gameObject.AddComponent<Rigidbody2D>(); }
            crb.gravityScale = 5;
            crb.mass = 10;
            crb.drag = 2;
            crb.AddForce(new Vector2(0,300),ForceMode2D.Impulse); 
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

        public bool NoTarget() {
            //if (Target != null) { Debug.Log("Target is" + Target.name); }
            //else { Debug.Log("No target!"); }
            if (Target == null) { return true; }
            return false;
        }

        bool death;
        float dtime=0.2f; //Deathtime
        private void Update()
        {
            if (death)
            {
                if (dtime > 0) { dtime -= Time.deltaTime; } else { Destroy(gameObject); }
            }
            
            //For use with Spawning FX, not much else.
        }

        private void LateUpdate()
        {
            //float dist = CalculateTarget();
            foreach (AIFunc aif in AIF)
            {
                
                if (insane) { aif.Execute(); } else { aif.ConditionCheck(); }
            }
        }

    }
}
