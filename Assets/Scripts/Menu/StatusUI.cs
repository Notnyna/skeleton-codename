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
        public Vector2 HPpos = new Vector2(10,10); //In local camera coords
        public GameObject HPBar;
        public GameObject AdditionalBar; //Chaos meter?

        private void Start()
        {
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            CameraT.TargetSwitched += CameraT_TargetSwitched;
            HPBar = Instantiate(HPBar,transform);
            HPBar.transform.localPosition = HPpos;
        }

        private void CameraT_TargetSwitched(Transform T)
        {
            //I wonder why and where is it worth checking, becoming delirious
            HPBar.GetComponent<HealthBar>().GetTarget(T);
        }

        private void FixedUpdate() //Makes it smoother, why it isn't is beyond me (for now)
        {
            transform.position = new Vector2(CameraT.transform.position.x, CameraT.transform.position.y);
        }
    }
}
