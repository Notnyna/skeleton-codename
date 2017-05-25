using UnityEngine;
using System.Collections.Generic;

namespace General
{
    /// <summary>
    /// Reservations:
    /// 0 1 2 - normal damaged
    /// 3 4 - ressist, block etc
    /// 5 6 - bleed
    /// </summary>
    public class SpawnFX : MonoBehaviour
    {
        public List<GameObject> debriprefab; //To do - ability to select specific debri
        //public float maxparticles;
        public float debrispeed;
        //private List<Transform> trackDebri;

        public void DoFX(float direction, Vector2 spawnpoint, float dev,float sdev, int[] pref, int burst=1)
        {

            for (int i = 0; i < burst; i++)
            {
                int di = Mathf.FloorToInt(Random.Range(0, pref.Length));
                //Debug.Log(di +  " is " + pref[di] + "  count " + debriprefab.Count);
                if (pref[di]>=debriprefab.Count) { Debug.Log("Cant find debri prefab " + pref[di]); return; }
                SpawnDebri(pref[di],spawnpoint, Random.Range(direction-dev,direction+dev),Random.Range(debrispeed-sdev,debrispeed+sdev)); //Random.Range(debrispeed-dev,debrispeed+dev));
            }
        }

        private void SpawnDebri(int i, Vector2 point, float dir, float speed)
        {
            if (debriprefab[i] == null) { return; }
            float flip = 0;
            if (transform.lossyScale.x > 0) { flip = 180; }
            dir =(dir+flip)* Mathf.Deg2Rad;
            GameObject d = Instantiate(debriprefab[i]);
            d.transform.position = point;
            Rigidbody2D drb = d.GetComponent<Rigidbody2D>();
            if (drb != null)
            {
                drb.AddForce(new Vector2(Mathf.Cos(dir),Mathf.Sin(dir))*speed,ForceMode2D.Impulse);
            }
        }

    }
}
