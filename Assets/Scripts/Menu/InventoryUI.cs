using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class InventoryUI : MonoBehaviour
    {
        private Item.ItemSlot[] Slots;
        private bool created = false;
        public Vector2 slotPos= new Vector2(-10,-10);
        public float slotPosSpace = 1f;
        public GameObject slotPrefab;

        private Transform Target;
        private Scenario.CameraControl CameraT;

        void Awake()
        {
            //Slots = new Item.ItemSlot[1];
            CameraT = Camera.main.GetComponent<Scenario.CameraControl>();
            if (CameraT == null) { Debug.Log("AAA, no camera!"); } //Worthless little snippets
            CameraT.TargetSwitched += CameraT_TargetSwitched;
        }

        private void CameraT_TargetSwitched(Transform T)
        {
            if (Target == T) { return; }
            created = false;
            if (Target != null) { Target.GetComponentInChildren<Character.Inventory>().OnChange -= InventoryUI_OnChange; }
            Target = T;
            OnEnable(); //Recreate inventory
        }

        private void OnEnable()
        {
            if (Target == null) { Target = CameraT.Target; if (Target == null) { return; } }
            if (!created)
            {
                Character.Humus H = Target.GetComponent<Character.Humus>();
                if (H == null) { return; } //Target not Human?
                CreateSlots(H.GetInventorySpace());
                AddInventory(H.GetInventory());
                created = true;

                Target.GetComponentInChildren<Character.Inventory>().OnChange += InventoryUI_OnChange; //Might be bad
                MakeSelected(H.HoldIndex());
            }
            //Debug.Log("Created succesful?");
        }

        private void InventoryUI_OnChange(Transform item,int index, bool removed)
        {
            if (item == null) { MakeSelected(index); return; }
            //AddInventory(Target.GetComponent<Character.Humus>().GetInventory());
            if (removed)
            {
                RemoveItem(item);
            }
            else { AddItem(item,index); }
            
        }

        private void AddInventory(Transform[] I)
        {
            if (I == null) { return; }
            for (int i = 0; i < I.Length; i++)
            {
                AddItem(I[i],i);
            }

        }

        private void CreateSlots(int slotAmount)
        {
            if (Slots!=null)
            {
                foreach (Item.ItemSlot slot in Slots)
                {
                    Destroy(slot.gameObject);
                }
            }

            Slots = new Item.ItemSlot[slotAmount];

            float xtent = slotPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x*slotPrefab.transform.localScale.x;
            for (int i = 0; i < slotAmount; i++)
            {
                Slots[i] = Instantiate(slotPrefab).GetComponent<Item.ItemSlot>();
                Slots[i].transform.SetParent(this.transform);
                Slots[i].transform.localPosition = new Vector3(slotPos.x + xtent * i+slotPosSpace*i, slotPos.y, 0);
            }
        }

        public bool AddItem(Transform item, int spot)
        {
            if (item == null) { return false; }
            //Check if it is not already added, should not occur in any case.
            for (int i = 0; i < Slots.Length; i++)
            {
                if  (Slots[i].GetHeldItem() == item)
                {
                    Debug.Log("Trying to add an existing item!" + item.name);
                    return false;
                }
            }
            //Add item to precisely that spot it is in Inv
            Slots[spot].PutItem(item);
            return true;
        }

        public bool RemoveItem(Transform item)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].GetHeldItem() == item)
                {
                    Slots[i].PutItem(null);
                    return true;
                }
            }
            return false;
        }

        private void MakeSelected(int hi) // Universal, but might be better to look at index of humus inv
        {
            if (!created) { return; }
            /*int hi = Target.GetComponent<Character.Humus>().HoldIndex();
            if (hi == -1) { Debug.Log("No inventory! should not call"); }*/

            foreach (Item.ItemSlot slot in Slots)
            {
                slot.Select(true);
            }

            Slots[hi].Select();
        }

        public void SwitchItems(int i, int l)
        {
            if (i > Slots.Length && l > Slots.Length) { return; }
            Transform t = Slots[i].PutItem(Slots[l].GetHeldItem());
            Slots[l].PutItem(t);
        }

        //private void Update()
        //{
           // transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y); //Why is it smooth?
        //}

    }
}