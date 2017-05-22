﻿using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class MoveAnimation : MonoBehaviour
    {
        /// <summary>
        /// x,y and timescale*z
        /// Next step to upgrade would be to store each animations in respective objects
        /// (trade storage for performance) (but heck there aren't that much animations to even amount to anything but go ahead)
        /// </summary>
        public string[] Animations; //Strings would still take less space
        private Vector4[] currentAni;

        private bool repeat = false; //Repeat current sequence?

        public float timescale = 1f; //To quickly hasten or slow animations

        public int AniIndex; // Current animation index

        public bool Animate;

        int S, F, C; //Start Finish Current
        float T; // Time on going for current sprite

        private Rigidbody2D RB;

        private void Start()
        {
            if (Animations==null) { Destroy(this); }
            RB = GetComponent<Rigidbody2D>();
            if (RB == null) { Debug.Log("Cant move animation without rigidbody yet, sorry!"); }

            if (Animate) { PlayAnimation(AniIndex, true, true); }
        }

        //Can make to play actual animations instead of vectors? wont need for now.

        public void PlayAnimation(int n, bool rep = false, bool force = false)
        {
            if (force | (n < Animations.Length & AniIndex != n))
            {
                // Debug.Log("Ani " + gameObject.name + " " + n);
                string[] parse = Animations[n].Split(' ');

                currentAni = new Vector4[parse.Length];

                string[] vparse;
                for (int i = 0; i < parse.Length; i++)
                {
                    try{
                        vparse = parse[i].Split(';');
                        if (vparse.Length < 3) { currentAni[i] = new Vector3(); }
                        else if (vparse.Length > 3)
                        {
                            currentAni[i] = new Vector4(
                                float.Parse(vparse[0]),
                                float.Parse(vparse[1]),
                                float.Parse(vparse[2]),
                                float.Parse(vparse[3])
                                );
                        }
                         else if (vparse.Length < 4)
                        {
                            currentAni[i] = new Vector4(
                                float.Parse(vparse[0]),
                                float.Parse(vparse[1]),
                                float.Parse(vparse[2]),
                                1
                                );
                        }
                    }  catch (System.Exception) { Debug.Log("Error in parsing mvani n="+n+" i=" +i); currentAni[i] = new Vector3(); }
                }
                F = parse.Length;
                S = 0;
                repeat = rep;
                C = 0; // Start animating from 0
                AniIndex = n; // Current animation index
                nextMove();
                //Debug.Log("Playing animation " + n + " in " + gameObject.name);
            }
        }

        public float currentAniTime()
        {
            //if (Animations[AniIndex] == null) { return 0; } Should not happen?
            if (currentAni == null) { return 0; }
            float time = 0;
            for (int i = 0; i < currentAni.Length; i++)
            {
                time += currentAni[i].w * timescale;
            }
            //Debug.Log("AniTime for " +gameObject.name + " : " + time);
            return time;
        }

        public int currentAniPos() { return C; }

        public void PlayAll()
        {
           // C = 0;
            //S = 0;
            //F = Animations.Length;
            //changeSprite();
        }

        public void goIdle()
        {
            //repeat = false;
            PlayAnimation(0);
            //currentAni = null;
        }

        public void nextMove()
        {
            if (currentAni == null) { return; } 
            //RB.MovePosition(new Vector2(RB.position.x+currentAni[C].x,RB.position.y+currentAni[C].y));
            //RB.MoveRotation(currentAni[C].z);
            //RB.AddTorque(currentAni[C].z);
            RB.AddForce(currentAni[C],ForceMode2D.Impulse);
            if (currentAni[C].z != 0) { mvRotate = currentAni[C].z; }
            T = currentAni[C].w * timescale;  //Set time for how long to wait until next
            C++;
            //Debug.LogFormat("Changed To {0}", Sprites[to].name);
        }

        private void Update()
        {
            T = T - Time.deltaTime;
            if (T < 0)
            {
                if (C < F) { nextMove(); }
                else
                {
                    if (!repeat) { goIdle(); }
                    else { C = S; nextMove(); }
                }
            }
        }
        private float mvRotate;
        private void FixedUpdate()
        {
            RB.MoveRotation(Mathf.Lerp(RB.rotation,mvRotate,Time.deltaTime/T));

        }

    }
}
