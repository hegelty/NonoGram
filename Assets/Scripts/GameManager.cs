using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lastLevel;
    public int stage;
    // Start is called before the first frame update
    void Start()
    {
        stage = PlayerPrefs.GetInt("Stage");
        if(stage==0) GetComponentInParent<Story>().ShowStory(0);
        else GetComponentInParent<Grid>().StartGame(stage);
    }

    public void ClearStage()
    {
        if(stage == lastLevel)
        {
            PlayerPrefs.SetInt("Stage", 0);
            GameClear();
        }
        else
        {
            GetComponentInParent<Story>().ShowStory(stage);
        }
    }
    
    public void ReadStory()
    {
        Debug.Log(stage);
        stage++;
        PlayerPrefs.SetInt("Stage", stage);
        GetComponentInParent<Grid>().StartGame(stage);
    }

    void GameClear()
    {
        Debug.Log("wa");
    }
}
