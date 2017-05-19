using UnityEngine;

namespace Item
{
    /// <summary>
    /// To use safely prime time of bullet damage must not hit the user.
    /// </summary>
    public class Gun : MonoBehaviour
    {
        public GameObject Bullet;
        public float bulletoffset=1;
        public int ammo;
        public float cooldown;
        private float counter;
        //private Character.Player Handler;
        private bool canfire;
        public float fireforce = 10;
        General.ListAnimation LS;

        private void Start()
        {
            LS = GetComponent<General.ListAnimation>();
            if (Bullet == null) { Debug.Log("Shooting nothing!"); }
        }

        /*private void OnEnable()
        {
            Handler = GetComponentInParent<Character.Player>();
            if (Handler != null) {
                Handler.OnActivate += Fire;
            }
        }*/
        public void Fire()//float direction) Should implement rotation if dont want to fire only according to gun
        {
            if (!canfire) { return; }

            GameObject b = Instantiate(Bullet);
            //float flip = -1;
            //if (transform.lossyScale.x < 0) { flip = 1; }
            Vector2 Boff = CalculateRotationUnitVector();
            b.transform.position = new Vector3(transform.position.x+Boff.x*bulletoffset, transform.position.y+Boff.y*bulletoffset,1);
            b.transform.rotation = transform.rotation;

            Rigidbody2D brb=b.GetComponent<Rigidbody2D>();

            if (transform.lossyScale.x < 0) { b.transform.localScale = new Vector2(-b.transform.localScale.x, b.transform.localScale.y); }

            brb.AddForce(
                Boff*fireforce
                ,ForceMode2D.Impulse);

            counter = cooldown;
            canfire = false;
        }


        private Vector2 CalculateRotationUnitVector()//float rotation)
        {
            //Calculate a unit vector in the direction of its rotation
            float flip = 0;
            if (transform.lossyScale.x > 0) { flip = 180; }
            float rotation = (transform.rotation.eulerAngles.z+flip)*Mathf.Deg2Rad;
            //Sin and cos have no idea what negative values are. Or maybe it is just me?
            Vector2 v = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
            return v;
        }

        /*private void OnDisable()
        {
            if (Handler != null)
            {
                Handler.OnActivate -= Fire;
            }
        }*/

        private void Update()
        {
            if (counter > 0) { counter -= Time.deltaTime; } else { canfire = true; }
        }

    }
}