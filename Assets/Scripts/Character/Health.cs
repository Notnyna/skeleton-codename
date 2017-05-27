using UnityEngine;

namespace Character
{
    /// <summary>
    /// Health system, tigthly connected to Humus
    /// Has stages.
    /// Set as global if belongs to a Humus.
    /// </summary>
    public class Health : MonoBehaviour //Would wish for each component to have individual health, no budget, no time :(
    {
        public delegate void HealthChange();
        public event HealthChange HpChanged;

        public bool Global; // Set true if it is destroyed when the character is destroyed. (Only one per Humus)
        public int HP = 4;

        private Humus H;
        private General.SpawnFX FX;

        private float bleedcount;

        private void Start()
        {
            if (Global) { H = GetComponent<Humus>(); }
            FX = GetComponent<General.SpawnFX>();
        }

        /// <summary>
        /// Deal damage to the component.
        /// 1-5 minor to no damage.
        /// 5-10 hits hard (think blunt).
        /// 10-20 pierce (bullets and piercing).
        /// 30-40 gibbing damage (explosions).
        /// </summary>
        /// <param name="dmg">Damage</param>
        /// <param name="location">Where FX to spawn</param>
        /// <param name="direction">Where FX to go</param>
        public void DealDamage(int dmg, Vector2 location,float direction)
        {
            
            //if (FX != null) { FX.DoFX(direction, location, 50,10, new int[]{ 0,1,2}); }
            if (HpChanged != null) { HpChanged(); }
        }

        public float GetPercentHP()
        {

            return 0;
        }

        private void Update()
        {
    
        }

    }
}
// 4 - Full health, full power, any hit will damage unless armor
// 3 - Wounded, cautious, full speed, hits will bleed and stagger, can take lots of damage or until critical hit based on armor
// 2 - Mortal, bleeding constantly, half speed
// 1 - Deathly coil, bleeding profusely, slow, cannot die unless critical hit.
// 
// Individual healths can be Critical or Minor
// If Critical hit, death by bleedout if not healed (depends on the critical). (eventually all is ejected)
// If Minor hit, 
// If there are non-global individual healths, the total health is 