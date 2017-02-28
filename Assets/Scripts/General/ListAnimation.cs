using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{

    public class ListAnimation : MonoBehaviour
    {
        public Sprite[] Sprites = null;
        public float[] STime = null;

        public bool AnimateAll = false; 
        public float timescale = 1f; //To quickly hasten or slow animations
        public bool repeat = false; //Repeat current sequence?
        private SpriteRenderer SR = null;
        private Sprite Def = null;

        public bool inAni = false; //Animating currently?

        int S, F, C; //Start Finish Current
        float T;

        private void Start()
        {
            SR = gameObject.GetComponent<SpriteRenderer>();
            if (SR == null && Sprites.Length < 1 && STime.Length != Sprites.Length)
            {
                gameObject.GetComponent<ListAnimation>().enabled = false;
            }
            Def = SR.sprite;
            if (AnimateAll) { PlayAll(); }
        }

        public void PlayAni(int s, int f)
        {
            if (!(s > 0 && f > 0 && Sprites.Length <= s && Sprites.Length <= f)) { return; }
            if (s > f) { int temp = s; s = f; f = temp; } // To do - animate backwards

            S = s; // To do - option whether to complete current animation first or skip
            F = f;
            T = STime[S] * timescale;

            changeSprite();

            inAni = true;
        }
        public void PlayAll()
        {
            C = 0;
            S = 0;
            F = Sprites.Length;
            T = STime[0] * timescale;

            inAni = true;
        }

        public void goIdle()
        {
            //To expand - idle animation 
            SR.sprite = Def;
            inAni = false;

        }

        public void changeSprite()
        {
            //Debug.LogFormat("Changed To {0}", Sprites[C].name);
            SR.sprite = Sprites[C];
            T = STime[C] * timescale;
            C++;
        }


        private void Update()
        {
            if (inAni)
            {
                T = T - Time.deltaTime;
                if (T < 0)
                {
                    if (C < F) { changeSprite(); }
                    else
                    {
                        if (!repeat) { goIdle(); }
                        else { C = S; changeSprite(); }
                    }


                }
                //Debug.LogFormat("{0}", T);
            }
        }

    }

}