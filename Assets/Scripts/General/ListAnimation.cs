using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{

    public class ListAnimation : MonoBehaviour
    {
        public Sprite[] Sprites = null;
        public string[] Animations = null;
        public float[] STime = null;

        public bool AnimateAll = false;  // To start sprite animating [DEBUG]
        public bool repeat = false; //Repeat current sequence?

        public float timescale = 1f; //To quickly hasten or slow animations
        public bool Multisprite = false; // If sprite belongs to spritesheet, check true
        public string PathFolder = "/"; // Folder name when going from resources/art (do not omit / / )
        private int[] currentAni = null;
        private SpriteRenderer SR = null;
        private Sprite Def = null;

        public bool inAni { private set; get; } //Animating currently?

        int S, F, C; //Start Finish Current
        float T; // Time on going for current sprite

        private void Start()
        {
            SR = gameObject.GetComponent<SpriteRenderer>();
            
            if (Multisprite) { FillSpritesList(); }

            if (Sprites.Length < 1) // If no sprites to animate, disable
            {
                Debug.Log("Animations not found for: "+gameObject.name);
                gameObject.GetComponent<ListAnimation>().enabled = false;
            }
            Def = SR.sprite;

            if (AnimateAll) { PlayAll(); }
        }

        void FillSpritesList() {
            string spname = SR.sprite.name; // ? does it actually get sprite name or gameobject ?
            spname = spname.Split('_')[0]; // To omit ex. "golem_0" to "golem"
            string path = "Art" + PathFolder + spname;
            Sprites = Resources.LoadAll<Sprite>(path);
            //Debug.Log(path);
        }


        public void PlayFromTo(int s, int f)
        {
            if (!(s > 0 && f > 0 && Sprites.Length <= s && Sprites.Length <= f)) { return; }
            if (s > f) { int temp = s; s = f; f = temp; } // To do - animate backwards

            S = s; // To do - option whether to complete current animation first or skip
            F = f;
            T = STime[S] * timescale;

            changeSprite();

            inAni = true;
        }

        public void PlayAnimation(int n, bool rep = false) {
            if (Animations != null && n < Animations.Length)
            {
                string ani = Animations[n];
                currentAni = new int[ani.Length]; //Setting up before play
                F = ani.Length;
                S = 0;
                C = 0;
                repeat = rep;
                for (int i = 0; i < ani.Length; i++)
                {
                    currentAni[i]=int.Parse(ani.Substring(i, 1));
                }
                inAni = true; // Start up animation
            }

        }

        public float currentAniTime() {
            if (currentAni == null) { return 0; }
            float time = 0;
            for (int i = 0; i < currentAni.Length; i++)
            {
                if (currentAni[i] >= STime.Length)
                {
                    time += timescale;
                }
                else {
                    time += STime[currentAni[i]] * timescale;
                }
            }
            return time;
        }

        public void PlayAll()
        {
            C = 0;
            S = 0;
            F = Sprites.Length;
            currentAni = null; // Temporary solution
            changeSprite();
            inAni = true;
        }

        public void goIdle()
        {
            //To expand - idle animation 
            SR.sprite = Def;
            inAni = false;
            //currentAni = null;
        }

        public void changeSprite()
        {
            int to = C;
            if (currentAni != null) { to = currentAni[C]; } // Gets the sprite from the sequence   
            SR.sprite = Sprites[to]; //Otherwise it is C

            if (to<STime.Length) // Array overflow prevention check
            {
                T = STime[to] * timescale;
            } else { T = timescale; }
            
            //Debug.LogFormat("Changed To {0}", Sprites[to].name);
            C++; // Go to next
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