using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class CharCreateScreen : MonoBehaviour
    {
        public GameObject DefaultChar;
        public GameObject[] Clothes;

        private General.Humus H;
        private Transform bodyparts;

        private int ci = 0;
        //private Transform Clothpart;

        public void OnEnable()
        {
            StartMenuCreation();
        }

        public void StartMenuCreation()
        {
            DefaultChar = Instantiate(DefaultChar);
            H = DefaultChar.GetComponent<General.Humus>();
            bodyparts = H.GetByType<General.StaticPos>().transform;
            H.gameObject.SetActive(true);
            Camera.main.GetComponent<General.CameraMove>().Target = H.gameObject;
        }

        public void OnDisable()
        {
            FinishCreation();
        }

        public void FinishCreation() {
            //Change Controls
            //MenuManager M = GetComponentInParent<MenuManager>();
            //General.Player Pcontrol = 
                H.gameObject.AddComponent<General.Player>();

            
            //

            //
            this.gameObject.SetActive(false);
        }

        public void ChangeClothes() {
            if (ci >= Clothes.Length) { ci = 0; }

            //Test only
            //Clothpart = 
                Instantiate(Clothes[ci],bodyparts);
            H.UpdateParts();
            ci++;
        }

    }
}