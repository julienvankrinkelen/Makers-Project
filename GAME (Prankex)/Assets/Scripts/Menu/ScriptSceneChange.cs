using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScriptSceneChange : MonoBehaviour
{
    
    bool loadingStarted = false;
    float secondsLeft = 0;
    void Start()
    {
        StartCoroutine(DelayLoadLevel(30));
    }
    IEnumerator DelayLoadLevel(float seconds)
    {
        secondsLeft = seconds;
        loadingStarted = true;
        do
        {
            yield return new WaitForSeconds(1);
        } while (--secondsLeft > 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void OnGUI()
    {
        if (loadingStarted)
            GUI.Label(new Rect(0, 0, 100, 20), secondsLeft.ToString());
    }
}
