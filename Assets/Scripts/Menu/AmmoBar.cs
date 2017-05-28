using UnityEngine;

namespace Menu
{
    public class AmmoBar : MonoBehaviour
    {
        private Item.Gun G;
        private Character.Inventory Inv;
        private General.Extender EX;

        private void Start()
        {
            EX = GetComponentInChildren<General.Extender>();
        }

        public void GetTarget(Transform T)
        {
            if (T == null) { Debug.Log("Should not happen?"); return; }
            G = T.GetComponent<Item.Gun>();
            //EX.ExtendPercent(0);
            //Debug.Log("changing targets to "+T.gameObject.name);
        }

        void CheckAmmo()
        {
            //int percent = Mathf.FloorToInt(((float)G.GetAmmo()/(float)G.clip)*100f);
            EX.ExtendPercent(G.GetAmmoPercent());
        }

        private void Update()
        {
            if (G != null) { CheckAmmo(); }
        }


    }
}
