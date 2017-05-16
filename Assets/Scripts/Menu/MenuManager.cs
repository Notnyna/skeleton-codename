using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// 0 - Main menu / pause
        /// 1 - Storymodemenu
        /// 2 - Inventory
        /// 3 - Status (always enabled) so should not be counted in?!
        /// Personally the best bet would be to have all UI/menus inherit abstract which has changeTargets
        /// </summary>
        private List<Transform> Menus;
        private int currmenu=0;
        public bool disableControl;
        public string openinv = "i";

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            Menus = new List<Transform>();
            foreach (Transform it in transform)  {
                if (it.CompareTag("Menu")) { Menus.Add(it); it.gameObject.SetActive(false); }
            }

            NoMenu();
        }

        public void NoMenu() {
            if (disableControl) { return; }
            Menus[currmenu].gameObject.SetActive(false);
            currmenu = 0;
        }

        public void ChangeMenu(int m) {
            if (disableControl) { return; }
            if (m>Menus.Count)
            {
                Debug.Log("Trying to open non existant menu! " + m);
                return;
            }
            if (Menus[m].gameObject.activeSelf) { Menus[m].gameObject.SetActive(false); }
            else { Menus[m].gameObject.SetActive(true); }
            currmenu = m;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { NoMenu(); }
            if (Input.GetKeyDown(openinv)) { ChangeMenu(0); }
            
        }

    }
}
