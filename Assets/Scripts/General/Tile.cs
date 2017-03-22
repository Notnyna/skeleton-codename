using System.Collections.Generic;
using UnityEngine;

namespace General
{

    public class Tile : MonoBehaviour
    {
        public float globalscale=10;
        SpriteRenderer sprite;
        int sizex;
        int sizey;
        public bool multitile=false;
        public bool allowflippingY = true;
        Sprite[] Tiles;

        void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            CalculateTileCount();

            if (multitile)
            {
                string sname = sprite.name.Remove(sprite.name.Length - 2);
                Tiles = Resources.LoadAll<Sprite>("Art/Ground/" + sname);
                //Debug.Log(P.Length);
                PutMultiTiles();
            }
            else {
                PutSingleTiles(); }

            sprite.enabled = false;
        }

        void PutMultiTiles() {
            GameObject P = new GameObject();
            SpriteRenderer Psprite = P.AddComponent<SpriteRenderer>();
            Psprite.sortingOrder = sprite.sortingOrder;
            // 0 - Surface, 1 - Edge, 2 - Core, 3 - Side


            // Surface
            Psprite.sprite = Tiles[0];
            for (int i = 1; i < sizex-1; i++) {
                CreateTile(P, ToTile(i, 0));
            }

            //Edges
            if (sizex > 1)
            {
                Psprite.sprite = Tiles[1];
                CreateTile(P, ToTile(sizex - 1, 0));
                Psprite.flipX = true;
                CreateTile(P, ToTile(0, 0));
                Psprite.flipX = false;
            }


            //Cores
            if (sizey > 1 && sizex > 1) {
                Psprite.sprite = Tiles[2];
                for (int i = 1; i < sizex-1; i++)
                {
                    for (int j = 1; j < sizey-1; j++)
                    {
                        CreateTile(P, ToTile(i, j));
                    }
                }
            }

            // Sides 
            if (sizey>2 && sizex>1)
            {
                Psprite.sprite = Tiles[3];
                //Right side
                for (int j = 1; j < sizey-1; j++)  {
                    CreateTile(P, ToTile(sizex-1, j));
                }
                //Left side
                Psprite.flipX = true;
                for (int j = 1; j < sizey - 1; j++)  {
                    CreateTile(P, ToTile(0, j));
                }
                Psprite.flipX = false;
            }

            if (sizey>1 && sizex>1)
            {
                //Edges (bottom)
                if (allowflippingY) {  Psprite.flipY = true;   }
                Psprite.sprite = Tiles[1];
                CreateTile(P, ToTile(sizex - 1, sizey-1));
                Psprite.flipX = true; 
                CreateTile(P, ToTile(0, sizey-1));
                Psprite.flipX = false; 

                // Bottom
                Psprite.sprite = Tiles[0];
                for (int i = 1; i < sizex - 1; i++)
                {
                    CreateTile(P, ToTile(i, sizey-1));
                }
                if (allowflippingY) { Psprite.flipY = true; }
            }

            //Pillar
            if (sizex==1)
            {
                Psprite.sprite = Tiles[0];
                CreateTile(P, ToTile(0,0));
                Psprite.sprite = Tiles[2];
                for (int i = 1; i < sizey; i++)
                {
                    CreateTile(P, ToTile(0, i));
                }
            }

            Destroy(P);
        }

        void PutSingleTiles() {
            GameObject P = new GameObject();
            SpriteRenderer Psprite = P.AddComponent<SpriteRenderer>();
            Psprite.sortingOrder = sprite.sortingOrder;
            Psprite.sprite = sprite.sprite;

            //Debug.Log(sizex+" : "+sizey);


            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    CreateTile(P,ToTile(i,j));
                }
            }


        }

        void CalculateTileCount()
        {
            Vector3 square = sprite.bounds.size;
            //Debug.Log(sprite.bounds.size.ToString());
            sizex = (int)Mathf.Round(square.x / (square.z * globalscale));
            sizey = (int)Mathf.Round(square.y / (square.z * globalscale));
        }

        void CreateTile(GameObject prefab, Vector2 pos) {
            GameObject child;
            child = Instantiate(prefab) as GameObject;
            child.transform.position = pos;
            child.transform.localScale = new Vector3(globalscale, globalscale);
            child.transform.parent = transform;
        }

        Vector2 ToTile(int posx, int posy)
        {
            Vector3 extentOffset = sprite.bounds.extents;
            float square = sprite.bounds.size.z * globalscale; //A single tile size, must be a square
            Vector3 parentOffset = transform.position;

            Vector2 P = new Vector2(square * posx - extentOffset.x + parentOffset.x + square/2,
             -square * posy + extentOffset.y + parentOffset.y - square/2
             );

            return P;
        }

    }
} 

