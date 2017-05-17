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
                MakeSelected();
            }
            //Debug.Log("Created succesful?");
        }

        private void InventoryUI_OnChange(Transform item, bool removed)
        {
            if (item == null) { MakeSelected(); return; }
            //AddInventory(Target.GetComponent<Character.Humus>().GetInventory());
            if (removed)
            {
                RemoveItem(item);
            }
            else { AddItem(item); }
            
        }

        private void AddInventory(Transform[] I)
        {
            foreach (Transform i in I)
            {
                AddItem(i);
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

        public bool AddItem(Transform item)
        {
            //Check if it is not already added, should not occur in any case.
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].GetHeldItem() == item)
                {
                    Debug.Log("Trying to add an existing item!" + item.name);
                    return false;
                }
            }

            //Add item to first empty spot
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].GetHeldItem() == null)
                {
                    Slots[i].PutItem(item);
                    return true;
                }
            }
            return false;
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

        private void MakeSelected()
        {
            int hi = CameraT.Target.GetComponent<Character.Humus>().heldItem;
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