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
    public class Health : MonoBehaviour
    {
        public delegate void HealthChange();
        public event HealthChange HpChanged;

        public int HP = 4;
        private Humus H;

        private void Start()
        {
            H=  GetComponent<Humus>();
            if (H == null) { Debug.Log("No humus, no health"); Destroy(this); }
        }

        public void DealDamage(int dmg)
        {
            HP -= dmg;
            if (HpChanged != null) { HpChanged(); }
        }
    }
}
