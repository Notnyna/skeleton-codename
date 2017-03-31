using UnityEngine;
using System.Collections;

namespace Scenario
{
    //http://answers.unity3d.com/questions/341350/how-to-fade-out-a-scene.html?page=2&pageSize=5&sort=votes
    public class FadeScreen : MonoBehaviour
    {
        public Texture2D fadeTexture;
        [Range(0.1f, 1f)]
        public float fadespeed;
        public int drawDepth = 0;

        private float alpha = 1f;
        private float fadeDir = -1f;

        void OnGUI()
        {
            if (fadeTexture==null) {
                Texture2D R = new Texture2D(1, 1);
                R.SetPixel(0, 0, Color.black);
                R.Apply();
                fadeTexture = R;
            }

            alpha += fadeDir * fadespeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color newColor = GUI.color;
            newColor.a = alpha;

            GUI.color = newColor;

            GUI.depth = drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

            if (alpha>=1)
            {
                Destroy(this);
            }
        }
    }
}
