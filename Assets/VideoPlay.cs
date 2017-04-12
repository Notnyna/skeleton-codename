using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour {
    private VideoPlayer videoPlayer;
    public VideoClip movie; 
    private RawImage image;
 	// Use this for initialization
	void Start () {
        var videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.source = VideoSource.VideoClip;
        StartCoroutine(playVideo());
        image = GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}


    IEnumerator playVideo()
    {

        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
      

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
       

        //We want to play from video clip not from url

        videoPlayer.source = VideoSource.VideoClip;
        


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = movie;
        videoPlayer.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound


        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }
        Debug.Log("Done Playing Video");
    }
}

