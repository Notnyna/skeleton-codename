﻿using UnityEngine;

namespace Menu
{
    public class StoryModeUI: MonoBehaviour
    {
        public GameObject Box;
        public Sprite[] Portraits;
        public GameObject TextM; //Must hold TextMesh
        private GameObject P; //Portrait
        private TextMesh TM; //TextMesh of TextM
        private string[] D; //Current dialogue
        private int Di; // Index of current line in array
        private int Li; // Index of current character in line
        private string T; //current text being written
        private float counter;
        public float letterspeed;
        private bool pause;
        private bool write;
        private bool waitfornext; // waiting for user input

        private void Awake()
        {
            if (TextM == null) { Debug.Log("No text!"); }
            TextM = Instantiate(TextM);
            TextM.transform.parent = transform;
            TM = TextM.GetComponent<TextMesh>();
            TM.GetComponent<Renderer>().sortingOrder = 200; //It works!

            Box = Instantiate(Box, transform); //Can I do that? of course.
            P = new GameObject("Portrait");
            SpriteRenderer Psp = P.AddComponent<SpriteRenderer>();
            Psp.sortingOrder = 100;
            P.transform.parent = transform;
            P.transform.localScale = new Vector3(5,5);
        }

        private void OnDisable()
        {
            Clear();
            DisablePlayer(true);
        }

        private void OnEnable()
        {
            //Calculates the width of screen and positions the box
            Vector2 xy = Box.GetComponent<SpriteRenderer>().sprite.bounds.size;
            float frustumWidth = (2.0f * 10 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad)) * Camera.main.aspect;

            Box.transform.localScale = new Vector2(frustumWidth/xy.x,3);
            Box.transform.localPosition = new Vector3(0, -frustumWidth / Camera.main.aspect + xy.y*4.5f, 0); //localscale.y+localscale.y/2 (center offset)
            //PortaitPos = 0;//new Vector2(Screen.currentResolution.width/4,1.5f); //Hardcoding is bad DYNAMIC HARDCODE EVERYTHING
            P.transform.localPosition = CalculateScreenPosition(new Vector2(65,-65));
            TextM.transform.localPosition = CalculateScreenPosition(new Vector2(-90,-45));
            DisablePlayer();
        }

        private void DisablePlayer(bool enable=false)
        {
            GameMaster GM = transform.parent.GetComponent<GameMaster>();
            if (GM == null) { Debug.Log("No game master!"); return; } 
            if (enable) { GM.EnableControls(); } else { GM.DisableControls(); }
        }

        private Sprite GetPortrait(string name)
        {
            for (int i = 0; i < Portraits.Length; i++)
            {
                if (string.Equals(Portraits[i].name, name,System.StringComparison.InvariantCultureIgnoreCase))
                { return Portraits[i]; }
            }
            return null;
        }

        private void CloseDialog()
        {
            DisablePlayer(true);
            //Change portrait to default unknown
            transform.parent.GetComponent<MenuManager>().NoMenu();
        }

        public void WriteDialog(string[] d, bool playercontrol=false) //Omitting playercontrol for now
        {
            D = (string[])d.Clone();
            Di = 0;

        }

        private void WriteText(string t)
        {
            T = t;
            write = true;
            Debug.Log("Writing: " +t);
        }

        private void WriteGo()
        {
            if (T.Length==0) {
                //Stop writing, reached end text'
                write = false;
                counter = letterspeed; // time before proceeding.
                return;
            }
            TM.text += T.Substring(0,1);
            T = T.Remove(0,1);
            counter = letterspeed;
        }

        private void DoNext()
        {
            if (Di >= D.Length) { Debug.Log("TimeToClose"); CloseDialog(); return; }
            if (D[Di].Length == 0) {
                //Continue with next line
                Di++;
                waitfornext = true;
                Debug.Log("Next Line!");
                return;
            }
            string cline = D[Di];
            Debug.Log("DoNext: " + cline);

            if (cline.StartsWith("\"")) {
                Debug.Log("1");
                cline = cline.Remove(0,1);;
                int last = cline.IndexOf("\"");
                WriteText(cline.Substring(0,last));
                cline = cline.Remove(0,last+1);
                D[Di] = cline;
                return;
            }
            if (cline.StartsWith("p")) { //Current max portraits - 99
                cline = cline.Remove(0,1);
                Debug.Log(cline.Substring(0, 2));
                int pi = int.Parse(cline.Substring(0,2));
                ChangePortrait(pi);
                cline = cline.Remove(0,2);
                D[Di] = cline;
                return;
            }
            if (cline.StartsWith("z")) { //Pause, max sleep time - 9.9
                Debug.Log("3");
                cline = cline.Remove(0,1);;
                float z = float.Parse(cline.Substring(0, 3));
                PauseDialogue(z);
                cline = cline.Remove(0,3);
                D[Di] = cline;
                return;
            }
        }

        private void PauseDialogue(float z)
        {
            Debug.Log("Pausing for "+ z);
            counter = z;
        }

        private void ChangePortrait(int p)
        {
            Debug.Log("Changing portrait to " + p);
            P.GetComponent<SpriteRenderer>().sprite = Portraits[p];
            //counter = letterspeed;
        }

        public void ActionNext()
        {
            counter = 0;
            if (waitfornext)
            {
                waitfornext = false;
                Clear();
            }
        }

        public void Clear()
        {
            TM.text = "";
        }

        private void Update()
        {
            if (Input.GetKey("f")) { ActionNext(); }

            if (counter > 0) { counter -= Time.deltaTime; }
            else {
                if (write) {
                    WriteGo();
                } else {
                    if (!waitfornext) { DoNext();
                    }
                }
            }
        }

        private Vector3 CalculateScreenPosition(Vector2 where)
        {
            var frustumh = (2.0f * 10 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));// * Camera.main.aspect;
            Vector3 pos = new Vector2(
                frustumh * Camera.main.aspect * where.x / 200,
                frustumh * where.y / 200
                );
            return pos;
        }
    }
}
