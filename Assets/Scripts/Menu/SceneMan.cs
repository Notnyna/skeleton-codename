using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour {

    public void quitapplication(string quit)
    {
        Application.Quit();
    }
    // Update is called once per frame
    public void scenechange(string changeto)
    {
        SceneManager.LoadScene(changeto);
    }
}
