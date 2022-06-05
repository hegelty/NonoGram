/*
 * 게임과 스크립트를 관리하기 위한 스크립트
 */
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int[] lastLevel = new int[3]; //마지막 레벨
    public int stage; //현재 스테이지
    public int storyId;
    
    //신 시작시
    void Start()
    {
        storyId = PlayerPrefs.GetInt("StoryId");
        stage = PlayerPrefs.GetInt("Stage" + storyId); //플레이어프렙스에서 스테이지 정보 가져오기
        if(stage==0) GetComponentInParent<Story>().ShowStory(storyId, 0); //처음 진행이면 0번 스토리 진행
        else GetComponentInParent<Grid>().StartGame(storyId, stage); //이니면 그냥 게임 시작
    }

    //노노그램 완성시
    public void ClearStage()
    {
        GetComponentInParent<Story>().ShowStory(storyId, stage); //스토리 보여주기
    }
    
    //스토리 다 읽었을 때
    public void ReadStory()
    {
        if(stage==lastLevel[storyId]) GameClear(); //모든 스테이지 종료시 종료
        stage++; //다음 스테이지로
        PlayerPrefs.SetInt("Stage" + storyId, stage); //스테이지 저장
        GetComponentInParent<Grid>().StartGame(storyId, stage); //다시 게임 시작
    }

    //게임 클리어시
    void GameClear()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene"); //타이틀 화면 복귀
    }
}
