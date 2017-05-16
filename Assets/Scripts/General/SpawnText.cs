using UnityEngine;

namespace General
{
    public class SpawnText: MonoBehaviour
    {
        public GameObject[] Texts;
        public Vector2 offset = new Vector2(0,0);
        public float speed;
        public float life;

        public void Spawn(int i)
        {
            if (Texts == null | Texts.Length < i) { Debug.Log("Spawning nothing at " + gameObject.name); return; }

            Vector2 spawnpos = new Vector2(transform.position.x,SearchMaxHeight());
            //GetComponent<SpriteRenderer>
            GameObject T = Instantiate(Texts[i]);
            T.transform.position = spawnpos+offset;
        }

        public float SearchMaxHeight()
        {
            SpriteRenderer SR = GetComponent<SpriteRenderer>();
            if (SR != null) { return SR.sprite.bounds.extents.y+transform.localPosition.y; }
            float x = 0;
            foreach (Transform part in transform)
            {
                SR=part.GetComponent<SpriteRenderer>();
                if (part.gameObject.activeSelf && SR != null)
                {
                    float t = SR.sprite.bounds.extents.y+part.transform.localPosition.y;
                    if (t > x) { x = t; }
                }
            }
            return x;
        }

    }
}
