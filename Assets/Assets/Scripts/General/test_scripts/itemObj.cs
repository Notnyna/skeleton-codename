using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class itemObj : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler{

    private Transform startParent;

    private CanvasGroup canvasGroup;

    private toolTip tooltip;

    private toolTipButton tooltipButton1, tooltipButton2;

    private bool inItem = false;

    
    public static GameObject currentItem; //TEST

    public static Image img;
    public int currentItemID;


    private void Start() {

       

        //Find CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();

        //Find toolTip
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<toolTip>();

        //tooltipButton2 = GameObject.FindGameObjectWithTag("TooltipButton2").GetComponent<toolTipButton>();

        startParent = transform.parent;
    }

    private void Update() {

        // when we right click on item
        if(Input.GetKeyDown(KeyCode.Mouse1) && inItem) {
            //Destroy(GameObject.FindGameObjectWithTag("currentItem")); // TEST
            tooltip.ToggleToolTip(true, Input.mousePosition);
            tooltipButton1 = GameObject.FindGameObjectWithTag("TooltipButton1").GetComponent<toolTipButton>();
            tooltipButton1.actionMethod = debugTest;
        }

       /* if(Input.GetKeyDown(KeyCode.Mouse0) && !inItem) {
            tooltip.ToggleToolTip(false, Input.mousePosition);
        }*/
    }

    private void debugTest() {
        Debug.Log("Use button");
    }


    public void OnBeginDrag(PointerEventData eventData) {

        transform.position = eventData.position;

        // Set new parent
        transform.SetParent(transform.parent.parent);

        // Disable BlockRaycast
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {

        getSlotInfo();
        // Enable BlockRaycast
        canvasGroup.blocksRaycasts = true;
    }

    private void getSlotInfo() {

        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");

        foreach(var slot in slots) {

            if(slot.GetComponent<slotEffect>().isSelected) {

                //Check if we already have an object in slot
                if(slot.transform.childCount > 0) {

                    // Getting itemObj of other slot
                    itemObj otheritem = slot.transform.GetChild(0).GetComponent<itemObj>();

                    // Caching other objects parent
                    Transform otherItemParent = otheritem.startParent;

                    // Set other items parent to ours
                    otheritem.startParent = startParent;

                    // Set our parent to cached parent
                    startParent = otherItemParent;

                    otheritem.transform.SetParent(otheritem.startParent); // Reset Parent of other item
                    otheritem.transform.localPosition = Vector2.zero; // Reset Position of other item
                }
                else {

                    startParent = slot.transform;         
                }

                break;
            }
        }
        transform.SetParent(startParent); // Reset Parent
        transform.localPosition = Vector2.zero; // Reset Position
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inItem = true;
        img = GetComponent<Image>();
        currentItem = this.gameObject; //save current item to use or remove

        //currentItem.tag = "currentItem"; //TEST
    }

    public void OnPointerExit(PointerEventData eventData) {
        inItem = false;
        //currentItem.tag = "Untagged";   //TEST
    }
}
