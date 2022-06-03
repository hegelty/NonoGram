/*
 * 게임 내 버튼에 관련된 스크립트
 */

using UnityEngine;

public class GameButton : MonoBehaviour
{
    //뒤로가기 버튼
    public void OnClickBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene"); //타이틀 화면으로 돌아가기
    }
}
