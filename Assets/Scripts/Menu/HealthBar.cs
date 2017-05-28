using UnityEngine;

namespace Menu
{
    public class HealthBar : MonoBehaviour
    {
        private Character.Health Target;
        //private General.ListAnimation LS;
        private Character.Inventory Inv;
        private General.Extender EX;

        private void Start()
        {
            //LS= GetComponent<General.ListAnimation>();
            //if (LS == null) { Debug.Log("No list animation? Its the best we have!"); }
            EX = GetComponentInChildren<General.Extender>();

        }

        public void GetTarget(Transform T)
        {
            if (T == null) { Debug.Log("Should not happen?"); return; }
            if (Target != null) { Target.HpChanged -= Target_HpChanged; }
            Target = T.GetComponent<Character.Health>();
            if (Target != null) {
                Target.HpChanged += Target_HpChanged;
            }
        }

        private void Target_HpChanged(Character.Health who)
        {
            EX.ExtendPercent(Target.GetPercentHP());
            //int hp = Target.HP;
            //if (hp > 3) { hp = 3; } //Simple, who need more ??
            //if (hp < 1) { hp = 1; }
            //LS.PlayAnimation(4-hp,true,true);
        }
    }
}