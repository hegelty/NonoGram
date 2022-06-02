using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    public void OnClickBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
