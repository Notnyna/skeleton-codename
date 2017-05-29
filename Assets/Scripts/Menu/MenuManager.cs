using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    /// <summary>
    /// keys: I wonder if it is safe to store all settings in a static class?
    /// forward - f (not managed here)
    /// exit(pause) - escape
    /// 0 - Main menu (exit - pause)
    /// 1 - Storymodemenu (exit - pause)
    /// 2 - Inventory (exit - exit)
    /// 3 - Status (always enabled) so should not be counted in?!
    /// Personally the best bet would be to have all UI/menus inherit abstract which has changeTargets
    /// TODO: Cannot access inventory when in storymode and main menu
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        public List<Transform> Menus = new List<Transform>();
        //private int currmenu;
        public bool disableControl;
        public string openinv = "i";


        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            /*Menus = new List<Transform>();
            foreach (Transform it in transform)  {
                if (it.CompareTag("Menu")) {
                    Menus.Add(it);
                    it.gameObject.SetActive(false);
                }
            }
            */
            NoMenu();
            ChangeMenu(0);
        }

        public void NoMenu() { //Might have to redo, this not only looks bad, it works bad
            if (disableControl) { return; }
            //if (Menus == null) { Debug.Log("No menus? why"); return; }
            if (Menus[0] != null) { Menus[0].gameObject.SetActive(false); }
            if (Menus[1] != null) { Menus[1].gameObject.SetActive(false); }
            if (Menus[2] != null) { Menus[2].gameObject.SetActive(false); }
            if (Menus[3] != null) { }// Menus[3].gameObject.SetActive(false); }
            //Menus[currmenu].gameObject.SetActive(false);
           // currmenu = 0;
        }

        /// <summary>
        /// 0 - Main menu (exit - pause)
        /// 1 - Storymodemenu (exit - pause)
        /// 2 - Inventory (exit - exit)
        /// </summary>
        /// <param name="m"></param>
        public void ChangeMenu(int m, bool dontdisable=false) {
            if (disableControl) { return; }
            //if (m>Menus.Count){ Debug.Log("Trying to open non existant menu! " + m);  return;  }
            if (Menus[m] == null) { return; }
            if (Menus[m].gameObject.activeSelf & !dontdisable) { Menus[m].gameObject.SetActive(false); }
            else { Menus[m].gameObject.SetActive(true); }
            //currmenu = m;
        }

        private void FixedUpdate()
        {
            transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }

        float count0 = 2;
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape)) { NoMenu(); }
            if (Input.GetKeyDown(openinv)) { ChangeMenu(2); }

            if (Input.GetKey(KeyCode.Escape) & !Menus[1].gameObject.activeSelf)
            {
                if (count0 > 0) { count0 -= Time.deltaTime; } else { Application.Quit(); }
                Debug.Log("Counting! " +count0);
            }
            else count0 = 2;
        }

    }
}
