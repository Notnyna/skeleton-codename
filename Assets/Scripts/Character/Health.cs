using UnityEngine;
using System.Collections.Generic;

namespace Character
{
    /// <summary>
    ///  Shit thats a lot of code
    /// </summary>
    public class Health : MonoBehaviour 
    {
        public delegate void HealthChange(Health who);
        public event HealthChange HpChanged; // For global or critical mainly

        public delegate void Death(Health who);
        public event Death OnDeath; // Global can use too, dunno how it works out, for parts mainly

        public delegate void SomethingDied(Transform who);
        public event SomethingDied OnPartDeath; // Only Global

       // public delegate void BleedingFull(Transform who);
       // public event BleedingFull OnBleedout; //Only Global (checks which bleedsout) Ok maybe health

        public bool Critical; //If it is not critical, it can be destroyed.
        public bool Global; // Set true if it has more than one health in children. But does not have hitbox itself. Should implement the other somehow.
        public int armor; //Reduce incoming damage by this
        public int RealHP = 400;
        private int CurrentHP;
        private int HPp = 100;
        //private int maxHp;
        public float fxmult=1;
        public bool diestwice;
        public bool diesnever;
        //private Humus H;
        private Rigidbody2D RB;
        private Rigidbody2D pRB; //Entirely for shields
        private General.SpawnFX FX;
        private List<Health> HPS;
        private List<Vector2> BLEED;
        public int maxbleed = 4;

        private void Awake()
        {
            FX = GetComponent<General.SpawnFX>();
            BLEED = new List<Vector2>();
            RB = GetComponent<Rigidbody2D>();
            pRB = GetComponentInParent<Rigidbody2D>();
            if (pRB == null)
            {
                if (transform.parent != null) { pRB = transform.parent.GetComponentInParent<Rigidbody2D>(); }
            }
            CurrentHP = RealHP;
            CalculatePercent();
            if (!Global) { return; }
            //H = GetComponent<Humus>();

            //AI.AIBase a = GetComponent<AI.AIBase>();

            //Player p = GetComponent<Player>();

            HPS = new List<Health>(GetComponentsInChildren<Health>(true));
            RealHP = 0;
            foreach (Health hp in HPS)
            {
                //Debug.Log("This is global, reporting " + hp.name);
                //maxHp += hp.HPmult * 100;
                if (hp != this)
                {
                    RealHP += hp.RealHP;
                    hp.HpChanged += hps_Changed;
                    hp.OnDeath += hps_OnDeath;
                }
            }
            CurrentHP = RealHP;
        }

        private void hps_OnDeath(Health who)
        {
            //Remove from event listener. Among other things.

            //Make dependant components informed that something broke
            if (OnPartDeath!= null) { OnPartDeath(who.transform); }

            if (who.Critical) {
                //Welp, time to die!
                //Destroy critical healths only
                foreach (Health hp in HPS)
                {
                    //maxHp += hp.HPmult * 100;
                    hp.HpChanged -= hps_Changed; //Can I remove twice?
                    hp.OnDeath -= hps_OnDeath;
                    hp.ExtremeGoreDeathFX(); 
                } 
                if (OnDeath != null) { OnDeath(this); }

                Destroy(this);
            }
            else
            {
                who.OnDeath -= hps_OnDeath;
                HPS.Remove(who);
            }
        }

        private void hps_Changed(Health who)
        {
            if (who.Critical)//If it is critical, %HP must not be higher than it.
            {
                if (HPp > who.HPp) //It is private, why can I access it!? Oh, because it is the same class?
                {
                    //Debug.Log("Critical has less health! from " + HPp + " to " + who.HPp );
                    ChangeHP(who.HPp);
                }
            }
            else {
                if (who.HPp != 100) { RawDamage(1); }
                
            } //Leave for later (make global hpp matters)
            
            //else { if (HpChanged != null) { HpChanged(this); } } //Might as well inform anyone that you've been hit
           // if (who.HPp < 50 | who.BLEED.Count>=who.maxbleed) {
           //     if (OnBleedout != null) { OnBleedout(who.transform); } //Is called numerious times when life changes, this is bad
            //}
            
        }

        public bool Dying()
        {
            if (HPp < 50 | BLEED.Count >= maxbleed)
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Deal damage to the component.
        /// 1-10 minor to no damage.
        /// 10-30 hits hard (think blunt).
        /// 30-60 pierce (bullets and piercing).
        /// >60 gibbing damage (explosions).
        /// </summary>
        /// <param name="dmg">Damage</param>
        /// <param name="location">Where FX to spawn</param>
        /// <param name="direction">Where FX to go</param>
        public bool DealDamage(int dmg, Vector2 location, Vector2 direction, float punch=0)
        {
            //Really needs a cleanup.
            bool pierce = false;
            dmg -= armor;
            if (Global) { Debug.Log("Dealing damage to global?! Should not happen! " + gameObject.name ); return false; }

            if (dmg < 10) // Minor hit, no bleeding, reflect FX (occasional), HP does not fall below 60%. (regenerates full if not bleeding). 
            {
                if (HPp > 60 && Random.Range(0, 5) == 0) // 1 in 4 chance
                {
                    pierce = true;
                }
                else { dmg = 0; }
                if (FX != null && Random.Range(0, 1) == 0) { FX.DoFX(direction, location, 50, 3, new int[] { 3, 4 }, Mathf.FloorToInt(1 * fxmult)); }
            }
            else if (dmg < 30) //Major hit, might start bleeding. Hard to kill.
            {
                if (HPp < 40)
                {
                    if (Random.Range(0, 5) == 0)
                    {
                        dmg = dmg / 2; pierce = true; Bleed(location);
                    }
                    else { dmg = dmg / 5; }
                }
                else if (Random.Range(0, 4) == 0) // 1 in 3 chance
                {
                    pierce = true;
                    Bleed(location);
                }
                else { dmg = dmg / 3; }

                if (FX != null) { FX.DoFX(direction, location, 30, 3, new int[] { 0, 1, 2, 3}, Mathf.FloorToInt(2 * fxmult)); }
            }
            else if (dmg < 50) //Mortal hit, will start bleeding to max. Wont die unless critical is hit.
            {
                pierce = true;
                if (HPp < 30) 
                {
                    int ndmg = dmg/2;
                    if (Critical)
                    {
                        if (Random.Range(0, 4) == 0) // 1 in 4 chance
                        {
                            ndmg = dmg;
                            pierce = false;
                        }
                    }
                    else
                    {
                        if (Random.Range(0, 2) == 0) // 1 in 2 chance
                        {
                            ndmg = dmg;
                            pierce = false;
                        }
                    }
                    dmg = ndmg;
                }
                Bleed(location);
                if (FX != null) { FX.DoFX(direction, location, 70, 5, new int[] { 0, 1, 2 }, Mathf.FloorToInt(3 * fxmult)); }
            }
            else // Brutal deadly hit, will bleed twice. Twice damage if all bleeding
            {
                pierce = true;
                Bleed(location);
                Bleed(location);
                if (BLEED.Count == maxbleed) { dmg += dmg; }
                if (FX != null) { FX.DoFX(direction, location, 50, 15, new int[] { 0, 1, 2 }, Mathf.FloorToInt(5 * fxmult)); }
            }

            //Deal damage depending not on percent!
            //dmg = 100 - (int)(((float)(RealHP - dmg) / (float)RealHP) * 100); 
            //if (dmg < 0) { dmg = 0; }
            CurrentHP -= dmg;
            CalculatePercent();
            if (punch != 0)
            {
                //float f = 1;
                //if (transform.lossyScale.x < 0) { f = -1; }
                if (RB != null) { RB.AddForceAtPosition(direction.normalized*punch, location, ForceMode2D.Impulse); }
                if (pRB != null) { pRB.AddForceAtPosition(direction.normalized * punch, location, ForceMode2D.Impulse); }
            }

            //if (BLEED.Count >= maxbleed) { deathflag = true; }


            //if (HpChanged != null) { HpChanged(this); }
            return pierce;
        }

        private void CalculatePercent()
        {
            HPp= (int)(((float)CurrentHP / (float)RealHP)*100f);
            if (HPp > 10 | BLEED.Count < maxbleed) { deathflag = false; } else { deathflag = true; }
            //Debug.Log("Bleeding at "+BLEED.Count +  deathflag.ToString());
            //Debug.Log("HP is " + CurrentHP + " %" + HPp);
            if (HpChanged != null) { HpChanged(this); }
        }

        private void ChangeHP(int p) //For when not damage related
        {
            if (p != HPp)
            {
                CurrentHP = (int)(((float)RealHP / 100f) * (float)p);
                CalculatePercent();
                
            }
        }

        private void RawDamage(int damage)
        {
            CurrentHP -= damage;
            CalculatePercent();
        }

        public int GetPercentHP()  { return HPp;  }

        private float count0;
        private float regen=2f;

        private float count1; //this one used for powerups (healkits)

        private bool deathflag;

        private void Update()
        {
            //To do, if its minor, regenerate is handled only in the global
            if (count0 > 0) { count0 -= Time.deltaTime; } else
            {
                Regenerate(); count0 = regen;
            }

            if (count1 > 0) { count1 -= Time.deltaTime; }
            else
            {
                BleedFX(); count1 = 0.3f;
            }
            
            if (deathflag && HPp < 0) {
                ExtremeGoreDeathFX();
            }
        }

        void ExtremeGoreDeathFX()
        {
            //Debug.Log("Death called? it's for " + gameObject.name + "  additional info " + HPp  );
            if (Global) { return;}
            if (FX != null)
            {
                FX.DoFX(new Vector2(0,-1), transform.position, 90, FX.debrispeed/3, new int[] { 0, 1, 2, 3, 4, 5, 6 }, Mathf.FloorToInt(20 * fxmult));
            }
            if (OnDeath != null) { OnDeath(this); }
            if (diesnever) { deathflag = false; ChangeHP(10); return; }
            if (diestwice) { diestwice = false; deathflag = false; ChangeHP(50);  return; }
            {
                gameObject.layer = 9;
                General.TimedDestroy td = gameObject.AddComponent<General.TimedDestroy>();
                td.timer = 3f;
                td.fxd = new int[] { 4, 5 };
                if (FX != null) { td.fx = true; }
                Destroy(this);
            }
        }

        void Regenerate(int amount=1) //natural regeneration if health is >50%
        {
            if (Global) { return; }
            float incpercent=0;
            //If bleeding and >50, start randomly healing bleeds
            if (HPp > 50)
            {
                if (BLEED.Count > 0)
                {
                    int r = Random.Range(0, 10);
                    if (r==0) //higher hp% increase chance of healing a bleed. Should, at least
                    {
                        //Debug.Log("Healing bleed");
                        Bleed(Vector2.zero, true);
                    }
                }
                incpercent += amount;
            }

            if (amount>1) //If using healing
            {
                if (Random.Range(0, 3)==0) // 1 in 3 chance 
                {
                    Bleed(Vector2.zero, true);
                }
                incpercent += amount; // heal anyway
            }

            if (BLEED.Count >= 0 && HPp>5) 
            {
                incpercent -= BLEED.Count; // deal damage if bleeding profusely
            }

            if (HPp == 100 && incpercent>0) { incpercent = 0;  }
            CurrentHP += (int)(((float)RealHP / 100f) * incpercent);

            CalculatePercent();
            //if (HPp < 10) { deathflag=true; }
            //if (HpChanged != null) { HpChanged(this); }
        }

        void Bleed(Vector2 P, bool unbleed= false)
        {
            if (!unbleed) // Increase bleeding
            {
                if (BLEED.Count >= maxbleed) { return; } //Cannot bleed more
                P = new Vector2(P.x-transform.position.x, P.y-transform.position.y );
                P *= 0.5f;
                BLEED.Add(P);
            }
            else //Decrease bleeding
            {
                if (BLEED.Count == 0) { return; } //Cannot unbleed anymore
                BLEED.RemoveAt(0);
                deathflag = false;
            }
        }

        void BleedFX()
        {
            if (BLEED.Count == 0) { return; }
            if (FX == null) { return; }
            foreach (Vector2 p in BLEED)
            {
                FX.DoFX(new Vector2(0,1),new Vector2(transform.position.x+p.x,transform.position.y+p.y),30,5,new int[] { 5,6 },1); //Maybe just one random?
            }
        }
    }
}
// Blunt hits cause slowdown, pierce does not unless melee (not managed here).
// 4 - Full health, full power.
// 3 - Wounded, cautious, full speed, hits will bleed and stagger, can take lots of damage or until critical hit or pierce
// 2 - Mortal, bleeding constantly, half speed
// 1 - Deathly coil, bleeding profusely, slow, cannot die unless critical hit.
// 0 - Bleeding until slow death, crawling without consciousness. 
//
// Individual healths can be Critical or Minor.
// If Critical >20, death by bleedout if not healed (depends on the critical). (eventually all is ejected)
// If Minor it is ejected after hitting below 0;
// If there are non-global individual healths, the total health is 