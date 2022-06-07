/*
 * 게임 내 버튼에 관련된 스크립트
 */

using UnityEngine;

public class GameButton : MonoBehaviour
{
    public GameObject helpPanel;
    //뒤로가기 버튼
    public void OnClickBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene"); //타이틀 화면으로 돌아가기
    }

    public void OnClickHelp()
    {
        var widthRatio = Screen.width / 1080f;
        GameObject panel = Instantiate(helpPanel, this.transform.parent.transform, true);
        panel.name = "HelpPanel";
        panel.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        panel.SetActive(true);
        
        GameObject closeButton = panel.transform.Find("CloseButton").gameObject;
        closeButton.name = "CloseButton";
        closeButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        closeButton.GetComponent<RectTransform>().sizeDelta =
            new Vector2(120 * widthRatio, 120 * widthRatio);
        closeButton.GetComponent<RectTransform>().localPosition =
            new Vector3((540  - 120) * widthRatio, (960 - 120) * widthRatio);
    }
    
    public void OnClickHelpExit()
    {
        Destroy(GameObject.Find("HelpPanel").gameObject);
    }

    public void OnClickWiki()
    {
        Application.OpenURL("https://gbs.wiki/w/노노그램");
    }
}
