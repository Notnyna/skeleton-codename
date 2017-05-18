﻿using UnityEngine;

namespace Menu
{
    public class HealthBar : MonoBehaviour
    {
        private Character.Health Target;
        private General.ListAnimation LS;

        private void Start()
        {
            LS= GetComponent<General.ListAnimation>();
            if (LS == null) { Debug.Log("No list animation? Its the best we have!"); }
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

        private void Target_HpChanged()
        {
            int hp = Target.HP;
            if (hp > 3) { hp = 3; }
            if (hp < 1) { hp = 1; }
            LS.PlayAnimation(4-hp,true,true);
        }
    }
}