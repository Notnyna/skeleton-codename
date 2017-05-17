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
        public GameObject HPBar;
        public GameObject AdditionalBar; //Chaos meter?

        private void Start()
        {
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            CameraT.TargetSwitched += CameraT_TargetSwitched;
            HPBar = Instantiate(HPBar,transform);
            //OnEnable();
        }

        private void OnEnable()
        {
            HPBar.transform.localPosition=CalculateScreenPosition(HPpos);
        }

        private Vector3 CalculateScreenPosition(Vector2 where) // Can make static since pretty useful
        {
            var frustumh = (2.0f * 10 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));// * Camera.main.aspect;
            Vector3 pos = new Vector2(
                frustumh*Camera.main.aspect*where.x / 200, 
                frustumh * where.y/200
                );
            return pos;
        }

        private void CameraT_TargetSwitched(Transform T)
        {
            //I wonder why and where is it worth checking, becoming delirious
            HPBar.GetComponent<HealthBar>().GetTarget(T);
        }

        //private void FixedUpdate() //Makes it smoother, why it isn't is beyond me (for now)
        //{
            //transform.position = new Vector2(CameraT.transform.position.x, CameraT.transform.position.y);
        //}
    }
}
