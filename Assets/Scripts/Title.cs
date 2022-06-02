using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    void Start()
    {
        if(!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 0);
        }
    }

    public void OnClickContinue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnClickRestart()
    {
        PlayerPrefs.SetInt("Stage", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickVote()
    {
        Application.OpenURL("http://game.hegelty.me/");
    }

    public void OnClickGithub()
    {
        Application.OpenURL("https://github.com/hegelty/nonogram");
    }
}