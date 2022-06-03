/*
 * 타이틀 화면에서 동작 스크립트
 */
using UnityEngine;

public class Title : MonoBehaviour
{
    //신 시작시
    void Start()
    {
        if(!PlayerPrefs.HasKey("Stage")) //플레이어프렙스에 스테이지 데이터가 없으면
        {
            PlayerPrefs.SetInt("Stage", 0); //초기화
        }
    }

    //이어하기 버튼 누를 때
    public void OnClickContinue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); //게임신으로 전환
    }

    //새로하기 버튼 누를때
    public void OnClickRestart()
    {
        PlayerPrefs.SetInt("Stage", 0); //스테이지 초기화
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); //게임신으로 전환
    }
    
    //종료 버튼 누를때
    public void OnClickExit()
    {
        Application.Quit();
    }

    //평가하러 가기 버튼 누를때
    public void OnClickVote()
    {
        Application.OpenURL("http://game.hegelty.me/");
    }

    //깃허브 버튼 누를때
    public void OnClickGithub()
    {
        Application.OpenURL("https://github.com/hegelty/nonogram");
    }
}