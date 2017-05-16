using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Reserved for characters(Humus) - 
    /// 0 Idle
    /// 1 Walk
    /// 2 Run
    /// 3 Action
    /// 4 Attack
    /// 5 Death
    /// </summary>
    public class ListAnimation : MonoBehaviour
    {
        public Sprite[] Sprites = null;
        public string[] Animations = null;
        public float[] STime = null;

        //public bool AnimateAll = false;  // To start sprite animating [DEBUG]
        private bool repeat = false; //Repeat current sequence?

        public float timescale = 1f; //To quickly hasten or slow animations

        public bool Multisprite = true; // If sprite belongs to spritesheet, check true
        public string PathFolder = "/"; // Folder name when going from resources/art (do not omit / / )

        private int[] currentAni = null;
        public int AniIndex; // Current animation index
        private SpriteRenderer SR = null;

        public bool Animate; 

        int S, F, C; //Start Finish Current
        float T; // Time on going for current sprite

        private void Start()
        {
            SR = gameObject.GetComponent<SpriteRenderer>();
            if (Multisprite) { FillSpritesList(); }
            //if (AnimateAll) { PlayAll(); }
            if (Animate) { PlayAnimation(AniIndex,true,true); }
        }

        void FillSpritesList() {
            string spname = SR.sprite.name; // ? does it actually get sprite name or gameobject ?
            spname = spname.Split('_')[0]; // To omit ex. "golem_0" to "golem"
            string path = "Art" + PathFolder + spname;
            Sprites = Resources.LoadAll<Sprite>(path);
            if (Sprites.Length < 1) // If no sprites to animate, disable
            {
                Debug.Log("Sprites not found for!!: " + path);
                Destroy(gameObject);
            }
        }

        public void PlayFromTo(int s, int f)
        {
            if (!(s > 0 && f > 0 && Sprites.Length <= s && Sprites.Length <= f)) { return; }
            if (s > f) { int temp = s; s = f; f = temp; }

            S = s; // To do option whether to complete current animation first or skip?
            F = f;
            T = STime[S] * timescale;

            changeSprite();

        }

        public void PlayAnimation(int n, bool rep = false, bool force=false) {
            if (force |( n < Animations.Length & AniIndex != n))
            {
               // Debug.Log("Ani " + gameObject.name + " " + n);
                string ani = Animations[n]; // Premade animation selected
                
                string[] anisprites = ani.Split(' ');

                currentAni = new int[anisprites.Length];
                F = anisprites.Length;
                S = 0;
                repeat = rep;
                

                for (int i = 0; i < F; i++)
                {
                    currentAni[i]=int.Parse(anisprites[i]);
                    if (currentAni[i]>Sprites.Length) { Debug.Log("Incorrect sprite index found, check animations for " + gameObject.name); }
                }

                C = 0; // Start animating from 0
                AniIndex = n; // Current animation index
                changeSprite();
                //Debug.Log("Playing animation " + n + " in " + gameObject.name);
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

        public int currentSprite() { return C; }

        public void PlayAll()
        {
            C = 0;
            S = 0;
            F = Sprites.Length;
            currentAni = null;
            changeSprite();
        }

        public void goIdle()
        {
            //repeat = false;
            PlayAnimation(0);
            //currentAni = null;
        }

        public void changeSprite()
        {
            int to = C; //Sprite is C (only used if animations=null)
            if (currentAni != null) { to = currentAni[C]; } // Gets the sprite from the sequence
            SR.sprite = Sprites[to];
            if (to < STime.Length) { T = STime[to] * timescale; } //Set time for how long to animate
            else { T = timescale; }
            C++;
            //Debug.LogFormat("Changed To {0}", Sprites[to].name);
        }

        private void Update()
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
        }

    }

}