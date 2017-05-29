using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PlayMovie : MonoBehaviour {
    public float w;
    private IEnumerator wait;
    public GameObject aud;
    bool canswitch;
    public KeyCode skip;
    // Use this for initialization
    void Start () {
        canswitch = false;
	}
	
	// Update is called once per frame
	void Update () {
        wait = waitfor(w);
        StartCoroutine(wait);
        if (Input.GetKeyDown(skip))
            canswitch = true;
        if(canswitch)
        SceneManager.LoadScene("Global");
	}

    IEnumerator waitfor(float len)
    {
        if (Time.time > 5)
            aud.SetActive(false);
        yield return new WaitForSeconds(len);
        canswitch = true;
    }
}
