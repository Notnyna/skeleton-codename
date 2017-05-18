using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Item
{
    public class ItemSlot : MonoBehaviour
    {

        private Transform InvItem; // The image shown on the slot
        private Transform RealItem=null; // The reference to the real item 
        public Sprite SelectedSprite;
        private Sprite DefSprite;
        public Vector3 InvItemScale = new Vector3(1.5f,1.5f,1);

        public bool isSelected;

        void Awake()
        {
            DefSprite = GetComponent<SpriteRenderer>().sprite;
            if (transform.childCount > 0) { return; }
            GameObject InvItemG = new GameObject("ItemInSlot");
            InvItem = InvItemG.transform;
            InvItem.transform.SetParent(transform);
            InvItem.transform.localPosition = new Vector3(0, 0, 0);
            InvItem.transform.localScale = InvItemScale;

            SpriteRenderer InvSpriteR = InvItemG.AddComponent<SpriteRenderer>();
            InvSpriteR.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        }

        public Transform GetHeldItem()
        {
            return RealItem;
        }

        public void Select(bool deselect=false)
        {
            if (isSelected & deselect) { GetComponent<SpriteRenderer>().sprite = DefSprite; isSelected = false; }
            if (!isSelected & !deselect) { GetComponent<SpriteRenderer>().sprite = SelectedSprite; isSelected = true; }
        }

        /// <summary>
        /// Returns the currently held item and puts the new one in.
        /// Can return null.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Transform PutItem(Transform item)
        {
            if (InvItem==null)
            {
                //foreach (Transform T in transform)
                //{
                //    InvItem =T; //Why does it lose reference to it? 
                //}
                // if (transform.childCount == 0)   { return null; }
                Debug.Log("No invItem for putting " + item.name);
            }
            Transform oldItem = RealItem;
            RealItem = item;
            if (item == null) {
                InvItem.GetComponent<SpriteRenderer>().sprite = null;
                return item;
            }
            InvItem.GetComponent<SpriteRenderer>().sprite = RealItem.GetComponent<SpriteRenderer>().sprite;
            return oldItem;
        }

    }
}