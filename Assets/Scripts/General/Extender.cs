using UnityEngine;
using System.Collections.Generic;

namespace General
{
    public class Extender: MonoBehaviour
    {
        public GameObject extensionPrefab;
        public GameObject extensionEnd;
        public Vector2 extendV;
        public int maxext; // maximum extension
        private int cext; //current extension count
        private List<Transform> EXTN; 

        public float cooldown;
        private float count;
        //private bool waiting;
        private int waiting;

        private void Start()
        {
            if (extensionEnd != null) {
                extensionEnd = Instantiate(extensionEnd,transform);
                PositionExtEnd();
            }
            EXTN = new List<Transform>();
        }

        private void PositionExtEnd()
        {
            if (extensionEnd == null) { return; }
            extensionEnd.transform.localPosition = extendV * (cext + 1);
        }

        private void Extend()
        {
            //if (waiting == 0) { return; }
            if (cext < maxext)
            {
                cext++;
                CreateExt(cext);
                PositionExtEnd();
                waiting--;
            }
        }

        private void Contract()
        {
            //if (waiting > 0) { waiting = 0; }
            if (cext > 0) {
                cext--;
                Destroy(EXTN[cext].gameObject);
                EXTN.RemoveAt(cext);
                PositionExtEnd();
            }
            waiting++;
        }

        private void CreateExt( int omult )
        {
            GameObject e = Instantiate(extensionPrefab,transform);
            e.transform.localPosition = extendV * omult;
            EXTN.Add(e.transform);
        }

        public void ExtendPercent(int percent)
        {
            int t = Mathf.FloorToInt(((float)maxext / 100f) * (float)percent);
            waiting =  t - cext;
        }

        private void Choose() {
            if (waiting > 0) { Extend(); }
            if (waiting < 0) { Contract();  }
            count = cooldown;
        }

        private void Update()
        {
            if (count > 0) { count -= Time.deltaTime; } else { Choose(); }
        }
    }
}
