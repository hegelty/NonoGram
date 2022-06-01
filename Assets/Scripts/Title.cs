using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickContinue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OnClickRestart()
    {
        PlayerPrefs.SetInt("Stage", 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    
    public void OnClickExit()
    {
        Application.Quit();
    }
}