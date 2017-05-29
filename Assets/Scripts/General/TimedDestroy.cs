using UnityEngine;

namespace General
{
    public class TimedDestroy : MonoBehaviour
    {
        public float timer=1f;
        public bool fx=false;
        public float fxtime = 0.2f;
        public int[] fxd = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        private SpawnFX FX;

        private void Start()
        {
            if (fx) { FX=GetComponent<SpawnFX>(); }
        }

        float fxcount;
        private void Update()
        {
            if (fx) {
                if (fxcount > 0) { fxcount -= Time.deltaTime; } else
                {
                    FX.DoFX(new Vector2(0,1),transform.position,90,10,fxd,1);
                    fxcount = fxtime;
                }
            }
            timer -= Time.deltaTime;
            if (timer < 0) { Destroy(gameObject); }
        }
    }
}
