/*
 * 스토리 진행에 관련한 스크립트
 */
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Story : MonoBehaviour, IPointerDownHandler
{
    public GameObject backGroundPrefeb; //반투명 배경 프리펩
    public GameObject storyTextPrefeb; //스토리 텍스트 프리펩
    private GameObject _backGround; //반투명 배경 오브젝트
    private GameObject _storyText; //스토리 텍스트 오브젝트
    private int _stageId; //스토리 번호
    private string[] _storyData; //스토리 텍스트 데이터
    private int _currentLine = 0; //현재 진행중인 스토리 라인
    private TextMeshProUGUI _text; //스토리 텍스트

    //스토리 재생 함수
    public void ShowStory(int storyId, int stageId)
    {
        var widthRatio = Screen.width / 540; //기준 가로 길이에 대한 가로 길이 비율
        _stageId = stageId; //스토리 번호 저장
        _currentLine = 0;
        
        //아무리 해도 배경이랑 스토리 텍스트들이 사각형 격자 밑으로 가서 여기서 생성함
        //반투명한 배경 인스턴스화
        //인스턴스의 부모를 이 오브젝트로 설정
        _backGround = Instantiate(backGroundPrefeb, this.transform, true);
        _backGround.transform.localPosition = new Vector3(0, 0, 0); //위치 설정
        _backGround.GetComponent<Image>().color = new Color(0, 0, 0, 180f/255f); //배경 색상 선택(반투명 검은색)
        
        //스토리 텍스트 인스턴스화
        //인스턴스의 부모를 이 오브젝트로 설정
        _storyText = Instantiate(storyTextPrefeb, this.transform, true);
        _storyText.GetComponent<TextMeshProUGUI>().fontSize = 24 * widthRatio; //텍스트 폰트를 가로 배율에 따라 설정
        _storyText.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, _storyText.GetComponent<RectTransform>().rect.height); //텍스트 영역을 화면 크기에 따라 설정
        _storyText.transform.localPosition = new Vector3(0,10000f/widthRatio,0); //텍스트의 아랫 부분이 가운데로 오도록 위치 설정
        _text = _storyText.GetComponent<TextMeshProUGUI>(); //텍스트 컴포넌트 저장
        _text.text = ""; //텍스트 초기화
        LoadStory(storyId, stageId); //스토리 불러오기
    }

    //스토리 불러오기
    private void LoadStory(int storyId, int stageId)
    {
        //Resources 폴더 안에 있는 파일 불러오기(Assets/Resources 폴더는 빌드시에 반드시 포함)
        var storyFile = Resources.Load<TextAsset>("Stories/story" + storyId + "/story_" + stageId);
        var sr = new StringReader(storyFile.text); //StringReader로 파일 내용을 읽어옴
        _storyData = sr.ReadToEnd()?.Split("\n"); //개행문자 기준으로 텍스트를 분할해 저장(Null 검사는 IDE가 하래서 함)
    }

    //스토리 진행
    public void OnPointerDown(PointerEventData eventData) //클릭 이벤트 발생
    {
        if(eventData.button == PointerEventData.InputButton.Left && _stageId >= 0) //좌클릭이면서 스토리 번호가 설정되어 있을때
        {
            if(_currentLine>=_storyData.Length) //모든 스토리를 다 읽었을 때
            {
                //텍스트랑 반투명 배경 삭제
                Destroy(_storyText);
                Destroy(_backGround);
                _stageId = -1; //스토리 번호 초기화
                GetComponentInParent<GameManager>().ReadStory(); //게임 메니저에서 스토리를 다 읽었을 때의 함수 호출
            }
            else //아직 스토리가 남았을 때
            {
                _text.text += "\n\n" + _storyData[_currentLine++]; //스토리 한 줄 출력
            }
        }
    }
}
