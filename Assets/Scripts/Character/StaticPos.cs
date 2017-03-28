using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Puts the children components into the specified local coordinates, going sequentally.
    /// Does not include grandchildren or this gameobj and skips if objects has staticposoverride.
    /// Can be overriden by StaticPosOverride
    /// -Used to minimize clutter when dealing with characters that need lots of precise placement.
    /// -Can use StaticPosOverride for even more precise placement.
    /// </summary>
    public class StaticPos : MonoBehaviour
    {
        public float[] posX = null;
        public float[] posY = null;

        void Start()
        {
            UpdatePositions();
        }

        public void UpdatePositions() {
            int i = 0;
            foreach (Transform child in transform)
            {
                StaticPosOverride P = child.GetComponent<StaticPosOverride>();
                if (P != null) { DoOverride(P); } // Do override first
                else // If no override, use from list.
                {
                    float x, y = 0;
                    if (i >= posX.Length) { x = 0; } else { x = posX[i]; }
                    if (i >= posY.Length) { y = 0; } else { y = posY[i]; }
                    child.localPosition = new Vector2(x, y);
                    i++;
                }
            }
        }

        private void DoOverride(StaticPosOverride P)
        {
            Transform Parent = P.GetParent();
            if (Parent != null)
            {
                P.transform.parent = Parent;
            }
            P.transform.localPosition = P.GetCoords();
        }
    }
}