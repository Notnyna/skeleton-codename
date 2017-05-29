using UnityEngine;
using System.Collections.Generic;

namespace General
{
    /// <summary>
    /// Reservations:
    /// 0 1 2 - normal damaged
    /// 3 4 - ressist, block etc
    /// 5 6 - bleed
    /// 7 - heal fx
    /// 
    /// 
    /// </summary>
    public class SpawnFX : MonoBehaviour
    {
        public List<GameObject> debriprefab; //To do - ability to select specific debri
        //public float maxparticles;
        public float debrispeed;
        //private List<Transform> trackDebri;
        public bool addvelocity = true;

        public void DoFX(Vector2 dir, Vector2 spawnpoint, float dev,float sdev, int[] pref, int burst=1)
        {

            for (int i = 0; i < burst; i++)
            {
                int di = Random.Range(0, pref.Length);
                float direction = Menu.UsefulStuff.FromVectorToRotation(dir);
                //Debug.Log(di +  " is " + pref[di] + "  count " + debriprefab.Count); Debug.Log("Cant find debri prefab " + pref[di]); 
                if (pref[di]>=debriprefab.Count) { return; }
                SpawnDebri(pref[di],spawnpoint,
                    Menu.UsefulStuff.FromRotationToVector(Random.Range(direction-dev,direction+dev),false),
                    Random.Range(debrispeed-sdev,debrispeed+sdev)
                    ); //Random.Range(debrispeed-dev,debrispeed+dev));
            }
        }

        private void SpawnDebri(int i, Vector2 point,Vector2  dir, float speed)
        {
            if (debriprefab[i] == null) { return; }
            //dir =(dir)* Mathf.Deg2Rad;
            GameObject d = Instantiate(debriprefab[i]);
            d.transform.position = point;
            d.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(-1,6);
            Rigidbody2D drb = d.GetComponent<Rigidbody2D>();
            if (drb != null)
            {
                Vector2 add = Vector2.zero;
                Rigidbody2D pRB = transform.GetComponent<Rigidbody2D>();
                if (pRB != null & !addvelocity) { add = pRB.velocity; }

                drb.AddTorque(Random.Range(-60,60));
                drb.AddForce(dir*speed+add,ForceMode2D.Impulse);
            }
        }

    }
}
