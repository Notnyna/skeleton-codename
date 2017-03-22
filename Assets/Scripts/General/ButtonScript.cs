using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class ButtonScript : MonoBehaviour
    {

        public bool button;

        // Update is called once per frame
        void Update()
        {

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Movable")
            {
                button = true;
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Movable")
            {
                button = true;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Movable")
            {
                button = false;
            }
        }
    }
}