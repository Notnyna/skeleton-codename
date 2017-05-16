using UnityEngine;

namespace Character
{
    public class Inventory : MonoBehaviour
    {
        public delegate void InventoryChanged(Transform item, bool removed);
        public event InventoryChanged OnChange;

        public int maxitems;

        public Transform[] GetInventory()
        {
            Transform[] I = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                I[i] = transform.GetChild(i);
            }
            return I;
        }

        public bool TakeItem(Transform item)
        {
            if (transform.childCount < maxitems)
            {
                item.SetParent(transform);
                item.transform.localPosition = new Vector3(0,0,transform.localPosition.z);
                item.gameObject.SetActive(false);
                if (OnChange != null) { OnChange(item,false); }
                return true;
            }
            return false;
        }

        public bool ReturnItem(Transform item)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (item==transform.GetChild(i))
                {
                    item.SetParent(null);
                    item.gameObject.SetActive(true);
                    //if (flip) { item.position = transform.position + new Vector3(-0.2f, 0, 0); }
                    //else { item.position = transform.position + new Vector3(0.2f, 0, 0); }
                    if (OnChange != null) { OnChange(item,true); }
                    return true;
                }
            } 
            return false;
        }

        public Transform ReturnItem(int i = -1)
        {
            if (transform.childCount < 1) { return null; }
            Transform drop;
            if (i == -1) { drop = transform.GetChild(0); }
            else { drop = transform.GetChild(i); }
            ReturnItem(drop);
            
            return drop;
        }

        public Transform SetHeld(int i)
        {
            Transform[] I = GetInventory();
            if (I.Length==0) { if (OnChange != null) { OnChange(null, false); } return null; }
            if (i>I.Length | i<0) { Debug.Log("Trying to hold too far"); }
            for (int l = 0; l < I.Length; l++)
            {
                if (l == i) { I[l].gameObject.SetActive(true); } else { I[l].gameObject.SetActive(false); }
            }
            if (OnChange != null) { OnChange(null,false); }
            return I[i]; //Why on earth is it bad?
        }

    }
}
