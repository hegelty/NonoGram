/*
 * 노노그램 플레이 관련 스크립트
 */
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static System.Int32;

public class Grid : MonoBehaviour
{
    public float everySquareOffset; //사각형 간 간격
    public GameObject gridSquare; //사각형
    public GameObject horizontalHintText; //수평 힌트
    public GameObject verticalHintText; //수직 힌트
    public GameObject heartImaage; //라이프 이미지
    public GameObject titleText; //스테이지 제목 텍스트
    private List<GameObject> _hearts = new List<GameObject>(); //라이프 인스턴스 리스트
    private List<GameObject> _gridSquares = new List<GameObject>(); //사각형 인스턴스 리스트
    private List<GameObject> _horizontalHintTexts, _verticalHintTexts; //힌트 리스트들
    public Vector2 startPosition, horizontalPosition, verticalPosition, heartPosition; //UI들 첫번째 위치
    public float hintTextOffset;
    private float _widthRatio; //가로 비율

    private int _mapId; //스테이지 맵 번호
    
    private int _mapSize, _lifes, _leftAnswers; //맵 크기, 남은 목숨, 남은 정답 칸
    private bool[,] _mapInfo; //맵 정보를 담을 2차원 bool 배열

    //신이 시작되었을 때
    private void Start()
    {
        _widthRatio = Screen.width / 540f; //기준 가로폭(540px)에 대한 가로 비율
    }

    //게임 시작
    public void StartGame(int mapId)
    {
        _mapId = mapId;
        if(mapId!=1) DestroyMap(); //첫 번째 맵이 아니면 맵 없애기
        LoadMapInfo(mapId); //파일에서 맵 읽어오기
        CreateGrid(); //사각형 격자 생성
        SpawnHeart(_lifes); //하트 생성
        SpawnHintText(); //힌트 텍스트 생성
    }

    //사각형 격자 생성
    private void CreateGrid()
    {
        SpawnGridSquare(); //사각형 격자 소환
        SetSquarePosition(); //사각형 격자 배치
    }
    
    //사각형 격자 소환
    private void SpawnGridSquare()
    {
        _gridSquares = new List<GameObject>(); //사각형 인스턴스 리스트 초기화
        for (int row = _mapSize-1; row >= 0; row--)
        {
            for (int col = 0; col < _mapSize; col++)
            {
                _gridSquares.Add(Instantiate(gridSquare, this.transform, true)); //사각형 인스터스 생성 후 리스트에 삽입, 해당 인스턴스의 부모를 이 오브젝트로 설정
                _gridSquares[_gridSquares.Count - 1].name = "Grid_" + row + "_" + col; //사각형 인스턴스 이름 설정
                _gridSquares[_gridSquares.Count - 1].GetComponent<Square>().Init(row, col, _mapInfo[row, col]); //사각형 인스턴스 값 설정
            }
        }
    }

    //사각형 격자 배치
    private void SetSquarePosition()
    {
        var squareRect = _gridSquares[0].GetComponent<RectTransform>(); //사각형 격자의 렉트 트랜스폼
        var offset = new Vector2(); //사각형 격자의 간격을 2차원 벡터로 초기화
        var rect = squareRect.rect; //렉트 트랜스폼에서 렉트
        var localScale = squareRect.transform.localScale; //사각형 격자의 로컬 스케일(배율)
        offset.x = (rect.width * localScale.x + everySquareOffset) * _widthRatio; //x축 간격
        offset.y = (rect.height * localScale.y + everySquareOffset) * _widthRatio; //y축 간격
        var offsetR = (_widthRatio - 1) * rect.width; //화면 배율에 따른 간격
        
        int colNum = 0, rowNum = 0; //현재 행, 열 번호
        foreach (var square in _gridSquares) //사각형 리스트의 모든 원소에 대해
        {
            if (colNum == _mapSize) //열이 맵 갯수만큼 되었을 때 줄바꿈
            {
                colNum = 0;
                rowNum++;
            }

            //최종적인 간격
            var posXOffset = offset.x * colNum - offsetR;
            var posYOffset = offset.y * rowNum - offsetR;
            square.GetComponent<RectTransform>().anchoredPosition = //캔버스의 앵커 기준으로 좌표 설정
                new Vector3(startPosition.x + posXOffset, startPosition.y + posYOffset);
            square.GetComponent<RectTransform>().localScale *= _widthRatio; //화면 비율에 맞춰 사각형 크기 배율 조절
            colNum++;
        }
    }

    //힌트 텍스트 생성
    private void SpawnHintText()
    {
        //힌트 텍스트 리스트 초기화
        _horizontalHintTexts = new List<GameObject>();
        _verticalHintTexts = new List<GameObject>();
        
        var squareRect = _gridSquares[0].GetComponent<RectTransform>(); //첫번째 사각형의 렉트 트랜스폼 가져오기
        var rect = squareRect.rect;
        var offsetR = (_widthRatio - 1) * rect.width; //사각형의 배율에 따른 간격 크기
        hintTextOffset += everySquareOffset * (_widthRatio - 1);
        
        //수직 힌트 추가
        for (var i = 0; i < _mapSize; i++)
        {
            var hintText = Instantiate(horizontalHintText, this.transform, true); //힌트 텍스트 인스터스 생성, 해당 인스턴스의 부모를 이 오브젝트로 설정
            _horizontalHintTexts.Add(hintText); //텍스트 인스턴스를 리스트에 삽입
            hintText.name = "HorizontalHintText_" + i; //이름 설정

            var text = hintText.GetComponent<TextMeshProUGUI>(); //텍스트 인스턴스의 TMP
            var textRect = hintText.GetComponent<RectTransform>(); //텍스트 인스턴스의 렉트 트랜스폼
            text.fontSize *= _widthRatio; //폰트를 가로 비율에 맞춰 키우기
            text.text = ""; //텍스트 초기화
            textRect.sizeDelta = new Vector2(120 * _widthRatio, 25 * _widthRatio); //택스트 창 크기를 배율에 따라 설정
            textRect.anchoredPosition = //캔버스의 앵커 기준으로 좌표 설정
                new Vector3(horizontalPosition.x - offsetR, horizontalPosition.y - hintTextOffset * i);

            var t = 0; //연결된 답 사각형 갯수를 저장할 변수
            for (var j = 0; j < _mapSize; j++)
            {
                if (t!=0&&!_mapInfo[i, j]) //이전 칸이 정답이지만 이 칸은 정답이 아닌 경우
                {
                    text.text += " " + (t); //힌트에 추가
                    t = 0; //연결된 답 사각형 갯수 초기화
                }
                else if (_mapInfo[i, j]) t++; //이 칸이 정답이면 연결된 갯수 한개 추가
            }
            if(t!=0) text.text += " " + (t); //마지막 칸이 정답이면 힌트로 추가
        }
        
        //수평 힌트 추가(수직 힌트와 동일한 방식)
        for (var i = 0; i < _mapSize; i++)
        {
            var hintText = Instantiate(verticalHintText, this.transform, true);
            _verticalHintTexts.Add(hintText);
            hintText.name = "VerticalHintText_" + i;
            
            var text = hintText.GetComponent<TextMeshProUGUI>();
            var textRect = hintText.GetComponent<RectTransform>();
            text.text = "";
            text.fontSize *= _widthRatio;
            textRect.sizeDelta = new Vector2(25 * _widthRatio, 120 * _widthRatio);
            textRect.anchoredPosition =
                new Vector3(verticalPosition.x - offsetR + hintTextOffset * i, verticalPosition.y);

            var t = 0;
            for (var j = 0; j < _mapSize; j++)
            {
                if (t!=0&&!_mapInfo[j, i])
                {
                    text.text += "\n" + (t);
                    t = 0;
                }
                else if (_mapInfo[j, i]) t++;
            }
            if(t!=0) text.text += "\n" + (t);
        }
    }
    
    //목숨 하트 생성
    private void SpawnHeart(int life)
    {
        _hearts = new List<GameObject>(); //하트 리스트 초기화
        for (var i = 0; i < life; i++)
        {
            _hearts.Add(Instantiate(heartImaage, this.transform, true)); //하트 이미지를 인스턴스화한 후에 리스트에 추가, 이 인스턴스의 부모 설정
            _hearts[i].name = "Heart_" + i; //이름 설정
            var rect = _hearts[i].GetComponent<RectTransform>(); //인스턴스의 렉트 트랜스폼
            //rect.localScale = new Vector3(rect.localScale.x * _widthRatio, rect.localScale.y * _widthRatio, 1); 
            rect.localScale *= _widthRatio; //하트의 크기를 가로 비율에 따라 설정
            
            //TODO 랙트 트랜스폼으로 변경
            rect.localPosition = //하트의 로컬 좌표 설정(근데 왜 로컬좌표로 짰는지 잘 모르겠음...)
                new Vector3(heartPosition.x - rect.rect.width * rect.localScale.x * i,
                    heartPosition.y - rect.rect.width * (_widthRatio - 1));
        }
    }

    //그림 제목 설정
    private void SetTitle(string title)
    {
        titleText.GetComponent<RectTransform>().localPosition = new Vector3(0, Screen.height * 0.85f, 0); //스크린 높이를 기준으로 위치 설정
        titleText.GetComponent<TextMeshProUGUI>().text = _mapId + ". " + title; //그림 제목 설정
        titleText.GetComponent<TextMeshProUGUI>().fontSize *= _widthRatio; //폰트 크기를 가로 비율에 따라 변경
    }

    //맵 정보 불러오기
    private void LoadMapInfo(int mapID)
    {
        //Resources 폴더 안에 있는 파일 불러오기(Assets/Resources 폴더는 빌드시에 반드시 포함)
        var mapFile = Resources.Load<TextAsset>("Maps/bin_" + mapID);
        var sr = new StringReader(mapFile.text); //불러온 TextAsset을 StringReader로 읽기
        SetTitle(sr.ReadLine()); //첫줄에 있는 그림 제목 불러와서 설정하기
        _mapSize = Parse(sr.ReadLine() ?? string.Empty); //두번째 줄 읽어서 int로 파싱(Null 검사는 IDE가 시켜서 함)
        _lifes = Parse(sr.ReadLine() ?? string.Empty); //세번째 줄 읽어서 int로 파싱(Null 22)
        _leftAnswers = 0; //남은 정답 갯수 0으로 설정
        _mapInfo = new bool[_mapSize, _mapSize]; //맵 정보 초기화
        for (var i = 0; i < _mapSize; i++)
        {
            var line = sr.ReadLine(); //한 줄 읽기
            for(var j=0;j<_mapSize;j++) //맵 크기만큼 단어 읽기
            {
                if (line != null) _mapInfo[i, j] = ((line[j] - '0') == 1); //글자가 1이면 맵에 true 표시(Null 검사 이것도 IDE가 시켜서 함)
                else _mapInfo[i, j] = false; //글자가 1이 아니면 맵에 false 표시
                _leftAnswers+=_mapInfo[i, j] ? 1 : 0; //맵이 정답이면 정답 개수 증가
            }
        }
    }
    
    //잘못된 칸을 선택했을 때
    public void MinusLife()
    {
        _lifes--; //목숨 하나 깍기
        
        if (_lifes <= 0) //목숨이 다 닳았으면
        {
            ResetMap(); //맵 초기화
        }
        else //아니면
        {
            _hearts[_lifes].SetActive(false); //하트 리스트에서 가장 마지막에 있는 하트를 비활성화
        }
    }
    
    //정답 칸을 선택했을 때
    public void CorrectAnswer()
    {
        _leftAnswers--; //남은 정답 칸 개수 감소
        if (_leftAnswers == 0) //남은 정답 칸이 없으면
        {
            GetComponentInParent<GameManager>().ClearStage(); //게임메니저에 있는 게임 클리어 함수 실행
        }
    }
    
    //맵 초기화
    private void ResetMap()
    {
        foreach (var square in _gridSquares) //사각형 리스트 안의 모든 원소에 대하여
        {
            square.GetComponent<Square>().Reset(); //해당 사각형이 가진 초기화 함수 실행
        }
        foreach (var heart in _hearts) //하트 리스트 안의 모든 원소에 대하여
        {
            heart.SetActive(true); //그 하트 활성화
        }
        LoadMapInfo(_mapId); //맵 다시 불러오기(남은 정답 칸 복구용)
    }

    //맵 전부 삭제
    private void DestroyMap()
    {
        foreach (var square in _gridSquares) //사각형 리스트 안의 모든 원소에 대하여
        {
            Destroy(square); //오브젝트 삭제
        }
        foreach (var hintText in _verticalHintTexts) //수직 힌트 리스트 안의 모든 원소에 대하여
        {
            Destroy(hintText); //오브젝트 삭제
        }
        foreach (var hintText in _horizontalHintTexts) //수평 힌트 리스트 안의 모든 원소에 대하여
        {
            Destroy(hintText); //오브젝트 삭제
        }
        foreach (var heart in _hearts) //하트 리스트 안의 모든 원소에 대하여
        {
            Destroy(heart); //오브젝트 삭제
        }
    }
}