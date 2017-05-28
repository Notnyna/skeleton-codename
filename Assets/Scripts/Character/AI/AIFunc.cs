using UnityEngine;

namespace Character.AI
{
    public interface AIFunc
    {
        void SetAIB(AIBase aib);
        void ConditionCheck();
        bool Execute();
        void Eject();
        void HalfPower();
    }
}
