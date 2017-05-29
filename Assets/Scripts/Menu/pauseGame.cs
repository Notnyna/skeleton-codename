using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseGame : MonoBehaviour {
    public GameObject pause;
    public KeyCode pausekey;
    private bool ispaused;
	// Use this for initialization
	void Start ()
    {
        ispaused = false;
        pause.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        pause.GetComponent<Transform>().position = GetComponent<Transform>().position;
        if (Input.GetKeyDown(pausekey))
        {    
            ispaused = !ispaused;
            pause.SetActive(ispaused);
        }
        if (ispaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
	}
}
