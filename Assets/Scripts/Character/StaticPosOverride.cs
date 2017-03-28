using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class StaticPosOverride : MonoBehaviour
    {
        public float X = 0;
        public float Y = 0;
        public int ParentType = 0;
        public GameObject Parent = null;
        public string ParentName = "X"; // Where to put this gameobj

        public Transform GetParent()
        {
            if (Parent != null)
            {
                return Parent.transform;
            }

            if (ParentName != "X")
            {
                Humus H = gameObject.GetComponentInParent<Humus>();
                if (H != null) { return H.GetByName(ParentName, ParentType); }
            }

            return null;
        }

        public Vector2 GetCoords()
        {
            return new Vector2(X, Y);
        }

    }
}