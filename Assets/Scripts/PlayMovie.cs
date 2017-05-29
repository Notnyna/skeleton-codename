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
    public VideoClip rev;
    bool reverse;
    // Use this for initialization
    void Start () {
        canswitch = false;
        reverse = true;
	}
	
	// Update is called once per frame
	void Update () {
        wait = waitfor(w);
        StartCoroutine(wait);
        if (Time.time > 5 && reverse)
        {
            GetComponent<VideoPlayer>().clip = rev;
            GetComponent<VideoPlayer>().Play();
            reverse = false;
        }
        if (Input.GetKeyDown(skip))
            canswitch = true;
        if(canswitch)
        SceneManager.LoadScene("Global");
	}

    IEnumerator waitfor(float len)
    {
        if (Time.time > 10)
            aud.SetActive(false);
        yield return new WaitForSeconds(2*len);
        canswitch = true;
    }
}
