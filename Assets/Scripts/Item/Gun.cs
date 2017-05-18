using UnityEngine;

namespace Item
{
    /// <summary>
    /// To use safely prime time of bullet damage must not hit the user.
    /// </summary>
    public class Gun : MonoBehaviour
    {
        public GameObject Bullet;
        public Vector2 BulletSpawnOffset= new Vector2(0,0);
        public int ammo;
        public float cooldown;
        private float counter;
        //private Character.Player Handler;
        private bool canfire;
        public bool automatic;
        public float fireforce = 10;

        private void Start()
        {
            if (Bullet == null) { Debug.Log("Shooting nothing!"); }
        }

        /*private void OnEnable()
        {
            Handler = GetComponentInParent<Character.Player>();
            if (Handler != null) {
                Handler.OnActivate += Fire;
            }
        }*/

        public void Fire(float direction)
        {
            if (!canfire) { return; }
            GameObject b = Instantiate(Bullet);
            float flip = -1;
            if (transform.lossyScale.x < 0) { flip = 1; }
            b.transform.position = new Vector3(transform.position.x+BulletSpawnOffset.x*flip, transform.position.y+BulletSpawnOffset.y,1);

            b.GetComponent<Rigidbody2D>().AddForce(
                CalculateRotationUnitVector(direction)*fireforce*-flip
                , ForceMode2D.Impulse);

            counter = cooldown;
            canfire = false;
        }

        private Vector2 CalculateRotationUnitVector(float rotation)
        {
            Vector2 v = -transform.right;
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