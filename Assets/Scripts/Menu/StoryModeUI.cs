using UnityEngine;

namespace Menu
{
    public class StoryModeUI: MonoBehaviour
    {
        public GameObject Box;

        private void Start()
        {
            Box = Instantiate(Box, transform); //Can I do that? of course.
        }

        private void OnEnable()
        {
            float x = Box.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            Box.transform.localScale = new Vector2(Screen.currentResolution.width/x,3);

        }

        public void WriteDialog(string[] d) { }
        public void WriteText(string s) { }
        public void ChangePortrait(Sprite sprite) { }
        public void Clear() { }

        private void Update()
        {
            transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        }
    }
}
