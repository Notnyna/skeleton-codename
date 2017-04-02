using System.Collections.Generic;
using UnityEngine;

namespace General
{

    public class Tile : MonoBehaviour
    {
        public float globalscale=10;
        SpriteRenderer SR;
        int sizex;
        int sizey;
        public bool multitile=false;
        public bool allowflippingY = true;
        Sprite[] Tiles;
        public string path = "Art/Ground/";

        void Awake()
        {
            SR = GetComponent<SpriteRenderer>();
            CalculateTileCount();

            if (multitile)
            {
                Tiles = Resources.LoadAll<Sprite>(path);
                if (Tiles.Length < 3)
                {
                    Debug.Log(path + "Returns less than 4 tiles!");
                }
                PutMultiTiles();
            }
            else { PutSingleTiles(); }

            SR.enabled = false;
        }

        void PutMultiTiles() {
            GameObject P = new GameObject();
            SpriteRenderer Psprite = P.AddComponent<SpriteRenderer>();
            Psprite.sortingOrder = SR.sortingOrder;
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
                Psprite.sprite = Tiles[1];
                if (allowflippingY) { Psprite.flipY = true; } else { Psprite.sprite = Tiles[2]; }
                CreateTile(P, ToTile(sizex - 1, sizey-1));
                Psprite.flipX = true; 
                CreateTile(P, ToTile(0, sizey-1));
                Psprite.flipX = false;

                // Bottom
                Psprite.sprite = Tiles[0];
                if (!allowflippingY) { Psprite.sprite = Tiles[2]; }
                for (int i = 1; i < sizex - 1; i++)
                {
                    CreateTile(P, ToTile(i, sizey-1));
                }

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
            Psprite.sortingOrder = SR.sortingOrder;
            Psprite.sprite = SR.sprite;

            for (int i = 0; i < sizex; i++)
            {
                for (int j = 0; j < sizey; j++)
                {
                    CreateTile(P, ToTile(i, j));
                }
            }

            Destroy(P);
        }

        void CalculateTileCount()
        {
            Vector3 bounds = SR.bounds.size;
            sizex = (int)Mathf.Round(bounds.x / (bounds.x*globalscale / transform.localScale.x));
            sizey = (int)Mathf.Round(bounds.y / (bounds.y*globalscale / transform.localScale.y));
            if (sizex < 0) { sizex = 1; }
            if (sizey < 0) { sizey = 1; }
            //Debug.Log(sizex+ " " + sizey + "   "+ SR.bounds.size.ToString() +"  Obj: " + gameObject.name);
        }

        void CreateTile(GameObject prefab, Vector2 pos)
        {
            GameObject child;
            child = Instantiate(prefab) as GameObject;

            child.transform.position = pos;
            child.transform.localScale = new Vector3(globalscale, globalscale);
            child.transform.parent = transform;

            SpriteRenderer childSR = child.GetComponent<SpriteRenderer>();
            childSR.color = SR.color;
            childSR.sortingOrder = SR.sortingOrder;
        }

        Vector2 ToTile(int posx, int posy)
        {
            Vector3 bounds = SR.bounds.size;
            float spritex = bounds.x * globalscale / transform.localScale.x; //A single tile size, must be a square?
            float spritey = bounds.y * globalscale / transform.localScale.y;

            Vector3 parentOffset = transform.position;
            Vector3 extentOffset = SR.bounds.extents;

            Vector2 P = new Vector2(
                spritex * posx - extentOffset.x + parentOffset.x + spritex / 2,
                -spritey * posy + extentOffset.y + parentOffset.y - spritey / 2
             );

            return P;
        }

    }
} 

