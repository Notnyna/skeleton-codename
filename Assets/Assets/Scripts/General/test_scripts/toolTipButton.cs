using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class toolTipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int fontsizeStart = 12;
    public int fontsizeAdd = 2;
    public int fontsizeNew;

    private Text text;

    private bool isInside = false;

    public Action actionMethod;


    private void Start() {
        text = GetComponent<Text>();
        fontsizeNew = fontsizeStart + fontsizeAdd;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0) && isInside) {
            setCallAction(actionMethod);
        }
    }

    public void setCallAction(Action action) {
        if(action != null) {
            action();
        }
        else {
            itemSpawner.droppedItemName = itemObj.img.sprite.name; // TEST
            itemSpawner.dropItem = true; // TEST
            Debug.Log(itemObj.img.sprite.name+" is dropped");
            Destroy(itemObj.currentItem); // TEST

        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        text.fontSize = fontsizeNew;
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData) {

        text.fontSize = fontsizeStart;
        isInside = false;
    }
    
}
