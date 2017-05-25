using UnityEngine;
using System.Collections.Generic;

namespace Character
{
    /// <summary>
    /// Should not interact with humus!
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="removed"></param>
        public delegate void InventoryChanged(Transform item, int index, bool removed);
        public event InventoryChanged OnChange;

        public int currentselect=0;
        public int maxitems=1;
        private Transform[] Inv;

        private void Start()
        {
            if (maxitems < 0) { Destroy(this); }
            Inv = new Transform[maxitems];
            for (int i = 0; i < transform.childCount; i++)
            {
                Inv[i] = transform.GetChild(i);
            }
        }

        public Transform[] GetInventory()
        {
            return Inv;
        }

        public bool TakeItem(Transform item)
        {
            if (item == null) { Debug.Log("taking NULL!"); return false; }
            if (transform.childCount < maxitems)
            {
                item.SetParent(transform);
                //item.transform.localPosition = new Vector3(0,0,transform.localPosition.z);
                item.gameObject.SetActive(false);
                int InvSpot = GetInvSpot();
                Inv[InvSpot] = item;
                if (OnChange != null) { OnChange(item,InvSpot,false); }
                return true;
            }
            return false;
        }

        private int GetInvSpot()
        {
            if (Inv[currentselect] == null) { return currentselect; } //Priority goes to selected square
            else for (int i = 0; i < Inv.Length; i++)
                {
                    if (Inv[i] == null) { return i; } //Put in the first empty spot
                }
            return -1; // Nowhere! Should not happen
        }

        public bool ReturnItem(Transform item)
        {
            for (int i = 0; i < Inv.Length; i++)
            {
                if (Inv[i]!= null & item==Inv[i])
                {
                    item.SetParent(null);
                    item.gameObject.SetActive(true);
                    //if (flip) { item.position = transform.position + new Vector3(-0.2f, 0, 0); }
                    //else { item.position = transform.position + new Vector3(0.2f, 0, 0); }
                    Inv[i] = null;
                    if (OnChange != null) { OnChange(item,i,true); }
                    return true;
                }
            } 
            return false;
        }

        public Transform ReturnItem(int i = -1)
        {
            if (transform.childCount < 1) { return null; }
            Transform drop;
            if (i == -1) { drop = Inv[currentselect]; }
            else { drop = Inv[i]; }
            ReturnItem(drop);

            return drop;
        }

        public Transform SetHeld(int i)
        {
            Transform[] I = GetInventory();
            if (i>=I.Length | i<0) { Debug.Log("Trying to hold too far " + i); i = 0; }
            currentselect = i;
            if (OnChange != null) { OnChange(null, i, false); }
            return I[i]; //Why on earth is it bad?
        }

    }
}
