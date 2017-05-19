using UnityEngine;

namespace Character
{
    /// <summary>
    /// Health system, tigthly connected to Humus
    /// Has stages.
    /// 4 - Full health, full power, any hit will damage unless armor
    /// 3 - Wounded, cautious, full speed, hits will bleed and stagger, can take lots of damage or until critical hit based on armor
    /// 2 - Mortal, bleeding constantly, half speed
    /// 1 - Deathly coil, bleeding profusely, slow, cannot die unless critical hit.
    /// </summary>
    public class Health : MonoBehaviour //Would wish for each component to have individual health, no budget, no time :(
    {
        public delegate void HealthChange();
        public event HealthChange HpChanged;

        public int HP = 4;
        private Humus H;
        private General.SpawnFX FX;

        private void Start()
        {
            H=  GetComponent<Humus>();
            FX = GetComponent<General.SpawnFX>();
            if (H == null) { Debug.Log("No humus, no health"); Destroy(this); }
        }

        public void DealDamage(int dmg, Vector2 location)
        {
            HP -= dmg;
            
            if (FX != null) { FX.DoFX(CalculateDirection(location), location, 50,10, 1); }
            if (HpChanged != null) { HpChanged(); }
        }

        private float CalculateDirection(Vector2 from) //Good enough for now ?
        {
            Vector2 line = new Vector2( transform.position.x,transform.position.y) - from;
            float ex = 180;
            if (from.y>transform.position.y) { ex = 0; }
            float d = Mathf.Atan2(line.y,line.x)*Mathf.Rad2Deg+ex;
            return d;
        }

    }
}
