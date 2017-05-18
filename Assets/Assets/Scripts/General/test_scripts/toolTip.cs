using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toolTip : MonoBehaviour {

    public Vector2 offset;

    public GameObject toolTipObject;

    public Image backgroundImage;
    public Text button1, button2;


    private void LateUpdate() {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            ToggleToolTip(false);
        }
         
    }

    public void ToggleToolTip(bool enable,Vector2 position) {
        if(enable) {
            toolTipObject.SetActive(true);
            backgroundImage.enabled = true;
            button1.enabled = true;
            button2.enabled = true;
        }
        else if(!enable) {
            toolTipObject.SetActive(false);
            backgroundImage.enabled = false;
            button1.enabled = false;
            button2.enabled = false;
        }

        transform.position = position + offset;
    }

    public void ToggleToolTip(bool enable) {
        if(enable) {
            toolTipObject.SetActive(true);
            backgroundImage.enabled = true;
            button1.enabled = true;
            button2.enabled = true;
        }
        else if(!enable) {
            toolTipObject.SetActive(false);
            backgroundImage.enabled = false;
            button1.enabled = false;
            button2.enabled = false;
        }

        transform.position = Vector2.zero;
    }
}
