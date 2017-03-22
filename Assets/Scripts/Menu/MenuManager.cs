using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        private Transform[] Menus;
        private int currmenu=0;

        private void Start()
        {

            Menus = new Transform[10];
            foreach (Transform it in transform)  {
                if (it.CompareTag("Menu")) { Menus[currmenu]=it; currmenu++; }
            }
            
            Transform[] temp = new Transform[currmenu];
            for (int i = 0; i < currmenu; i++)  { temp[i] = Menus[i]; Menus[i].gameObject.SetActive(false); } //Disable all menus
            Menus = temp;
            currmenu = 0; 

            if (Menus.Length >= 1) { ChangeMenu(0); }
        }

        public void NoMenu() {
            Menus[currmenu].gameObject.SetActive(false);
            currmenu = 0;
        }

        public void ChangeMenu(int m) {
            Menus[currmenu].gameObject.SetActive(false);
            Menus[m].gameObject.SetActive(true);
            currmenu = m;
        }


    }
}
