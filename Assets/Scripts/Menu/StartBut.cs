using UnityEngine;

namespace Menu
{
    public class StartBut : MonoBehaviour
    {
        public Scenario.ToMouse M;
        MenuManager MM;
        GameMaster GM;

        private void Awake()
        {
            MM = FindObjectOfType<MenuManager>();
            GM = FindObjectOfType<GameMaster>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0)) { if (M.MouseTarget == transform) { MM.NoMenu();  GM.SwitchPlayer(0); } }

        }

    }
}
