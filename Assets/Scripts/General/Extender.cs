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
        public bool EndisPrefab = true;
        public int minext=0;

        private void Start()
        {
            if (EndisPrefab && extensionEnd != null)
            {
                extensionEnd = Instantiate(extensionEnd,transform);
                PositionExtEnd();
            }
            else { PositionExtEnd(); }
            EXTN = new List<Transform>(maxext);
            if (extensionPrefab == null) {
                Debug.Log("No extension prefab?" + gameObject.name); }

            for (int i = 1; i < maxext+1; i++)
            {
                CreateExt(i);
            }
            cext = maxext-1;
            ExtendPercent(0);
            //waiting=minext;
        }

        private void PositionExtEnd()
        {
            if (extensionEnd == null) { return; }
            //if (Vector2.Distance(extensionEnd.transform.localPosition, extendV * cext) >1) {
            //    extensionEnd = null; return; }
            extensionEnd.transform.localPosition = extendV * (cext + 1);
        }

        private void Extend()
        {
            //if (waiting == 0) { return; }
            if (cext < maxext)
            {
                //CreateExt(cext);
                if (cext < maxext - 1) { cext++; }
                EXTN[cext].gameObject.SetActive(true);
                PositionExtEnd();
                
            }
            waiting--;
        }

        private void Contract()
        {
            //if (waiting > 0) { waiting = 0; }
            if (cext> minext) {
                
                //Destroy(EXTN[cext].gameObject);
                //EXTN.RemoveAt(cext);
                EXTN[cext].gameObject.SetActive(false);
                PositionExtEnd();
                if (cext > minext) { cext--; }
            }
            waiting++;
        }

        private void CreateExt( int omult )
        {
            GameObject e = Instantiate(extensionPrefab,transform);
            e.transform.localPosition = extendV * omult;
            EXTN.Add(e.transform);
            //if (destroy) { e.AddComponent<TimedDestroy>(); }
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

        //bool destroy = false;
        private void OnDestroy()
        {
            waiting = 0;
            //destroy = true;
            //foreach (Transform e in GetComponentInChildren<Transform>())
            //{
                //e.gameObject.AddComponent<TimedDestroy>();
            //}
        }
    }
}
