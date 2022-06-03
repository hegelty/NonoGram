/*
 * 사각형의 동작에 관한 스크립트
 */
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

struct SquareState //조건을 맞추기 위해 만든 구조체
{
    public bool IsPainted; //색칠되어 있는지
    public bool IsChecked; //체크(아닌곳)되어 있는지
    public bool IsWrong; //오답인지
    
    public void SetState(bool isPainted, bool isChecked, bool isWrong) //상태 설정
    {
        IsPainted = isPainted;
        IsChecked = isChecked;
        IsWrong = isWrong;
    }
    
    public bool CanPaint() //색칠할 수 있는지 여부
    {
        return !IsPainted && !IsChecked && !IsWrong;
    }
};

public class Square : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private SquareState _state; //이 사각형 상태
    public GameObject square; //사각형 오브젝트
    public Image image; //사각형 오브젝트의 이미지
    public Sprite baseImage, ansImage, wrongImage, checkImage; //사각형 상태에 따른 이미지
    public int row, col; //사각형의 행과 열 정보
    public bool ans; //이 사각형이 정답인지
    void Start()
    {
        image = square.GetComponent<Image>(); //사각형 오브젝트의 이미지를 가져옴
    }

    //사각형 설정
    public void Init(int x, int y, bool ans)
    {   
        row = x;
        col = y;
        this.ans = ans;
        SetImage(0); //이미지를 기본 상태로
    }

    //사각형 초기화
    public void Reset()
    {
        _state.SetState(false, false, false); //상태 초기화
        SetImage(0); //이미지를 기본 이미지로 설정
    }

    //사각형 색칠, 체크
    //드래그 처리 함수
    public void OnPointerEnter(PointerEventData eventData) //마우스포인터가 사각형에 진입했을때
    {
        if(Input.GetMouseButton(0) && _state.CanPaint()) //좌클릭이 눌려있고 색칠이 가능하면
        {
            if (ans) //정답일때
            {
                SetImage(1); //정답 이미지 설정
                GetComponentInParent<Grid>().CorrectAnswer(); //정답 칸 갯수 감소
            }
            else //틀렸을때
            {
                SetImage(2); //틀린 이미지 설정
                _state.IsWrong = true; //오답 상태로 설정
                GetComponentInParent<Grid>().MinusLife(); //생명 감소
            }
            _state.IsPainted = true; //색칠된 상태로 설정
        }
        else if (Input.GetMouseButton(1) && _state.CanPaint()) //우클릭이 눌려있고 색칠이 가능하면
        {
            SetImage(3); //체크 이미지 설정
            _state.IsChecked = true; //체크 상태로 설정
        }
        else if (Input.GetMouseButton(1) && _state.IsChecked) //우클릭이 눌려있고 체크된 상태이면
        {
            SetImage(0); //기본 상태로 변경
            _state.IsChecked = false; //체크 해제
        }
    }

    //클릭 처리
    public void OnPointerDown(PointerEventData eventData) //클릭시
    {
        //드래그시 함수와 동일
        if(eventData.button == PointerEventData.InputButton.Left && _state.CanPaint())
        {
            if (ans)
            {
                SetImage(1);
                GetComponentInParent<Grid>().CorrectAnswer();
            }
            else
            {
                SetImage(2);
                _state.IsWrong = true;
                GetComponentInParent<Grid>().MinusLife();
            }
            _state.IsPainted = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _state.CanPaint()) //우클릭
        {
            SetImage(3);
            _state.IsChecked = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _state.IsChecked) //우클릭
        {
            SetImage(0);
            _state.IsChecked = false;
        }
    }

    //이미지 설정
    //0: 기본, 1: 정답, 2: 오답, 3: 체크
    public void SetImage(int imageType)
    {
        switch(imageType) {
            case 0:
                image.sprite = baseImage;
                break;
            case 1:
                image.sprite = ansImage;
                break;
            case 2:
                image.sprite = wrongImage;
                break;
            case 3:
                image.sprite = checkImage;
                break;
        }
    }
}