using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lastLevel;
    public int stage;
    void Start()
    {
        stage = PlayerPrefs.GetInt("Stage");
        if(stage==0) GetComponentInParent<Story>().ShowStory(0);
        else GetComponentInParent<Grid>().StartGame(stage);
    }

    public void ClearStage()
    {
        GetComponentInParent<Story>().ShowStory(stage);
    }
    
    public void ReadStory()
    {
        if(stage==lastLevel) GameClear();
        stage++;
        PlayerPrefs.SetInt("Stage", stage);
        GetComponentInParent<Grid>().StartGame(stage);
    }

    void GameClear()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
