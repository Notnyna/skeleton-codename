using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class slotEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public Outline outline;

    public bool isSelected;

    void Start()
    {
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
        isSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
        isSelected = false;
    }

 

}
