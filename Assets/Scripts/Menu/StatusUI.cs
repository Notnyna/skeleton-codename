using UnityEngine;

namespace Menu
{
    /// <summary>
    /// How to handle different UI for like (humans/monsters/apparatus)?
    /// Think handling it here is best.
    /// </summary>
    public class StatusUI : MonoBehaviour
    {
        Scenario.CameraControl CameraT;
        public Vector2 HPpos = new Vector2(0,60); //In percent starting from center x=-100 y=-100 is bottom left
        public Vector2 Ammopos = new Vector2(0, 50);
        public GameObject HPBar;
        public GameObject ABar;
        public GameObject AdditionalBar; //Chaos meter?

        private Character.Inventory Inv; //For ammo

        private void Start()
        {
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            CameraT.TargetSwitched += CameraT_TargetSwitched;
            //Inv = CameraT.Target.GetComponent<Character.Humus>().GetComponentInChildren<Character.Inventory>();
            //if (Inv != null) { Inv.OnChange += Inv_OnChange; }
            if (HPBar != null) { HPBar = Instantiate(HPBar, transform); }
            if (ABar != null) { ABar= Instantiate(ABar, transform); }
            OnEnable();
            
        }

        private void Inv_OnChange(Transform item, int index, bool removed)
        {
            if (item == null)
            {
                item = Inv.GetInventory()[index];
                if (item == null) { ABar.SetActive(false); return; }
                Item.Gun G= item.GetComponent<Item.Gun>();
                if (G == null) { ABar.SetActive(false); return; }
                else {
                    ABar.SetActive(true);
                    ABar.GetComponent<AmmoBar>().GetTarget(item);
                }
            }
        }

        private void OnEnable()
        {
            HPBar.transform.localPosition=UsefulStuff.CalculateScreenPosition(HPpos);
            ABar.transform.localPosition = UsefulStuff.CalculateScreenPosition(Ammopos);
        }

        private void CameraT_TargetSwitched(Transform T)
        {
            if (Inv != null) { Inv.OnChange -= Inv_OnChange; }
            //I wonder why and where is it worth checking, becoming delirious
            HPBar.GetComponent<HealthBar>().GetTarget(T);
            //ABar.GetComponent<AmmoBar>().GetTarget(T);
            Character.Humus H = T.GetComponent<Character.Humus>();
            if (H == null) {
                ABar.SetActive(false);
                Inv = null;
                return;
            }
            Inv = H.GetComponentInChildren<Character.Inventory>();
            if (Inv != null) { Inv.OnChange += Inv_OnChange; }
        }

        //private void FixedUpdate() //Makes it smoother, why it isn't is beyond me (for now)
        //{
            //transform.position = new Vector2(CameraT.transform.position.x, CameraT.transform.position.y);
        //}
    }
}
