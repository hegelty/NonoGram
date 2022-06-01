using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

struct Sqaure_State
{
    public bool _isPainted; //색칠되어 있는지
    public bool _isChecked; //체크(아닌곳)되어 있는지
    public bool _isWrong; //오답인지
    
    public void setState(bool isPainted, bool isChecked, bool isWrong)
    {
        _isPainted = isPainted;
        _isChecked = isChecked;
        _isWrong = isWrong;
    }
    
    public bool canPaint()
    {
        return !_isPainted && !_isChecked && !_isWrong;
    }
};

public class Square : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private Sqaure_State _state;
    public GameObject square;
    public Image image;
    public Sprite baseImage, ansImage, wrongImage, checkImage;

    public int row, col;
    public bool ans;
    void Start()
    {
        image = square.GetComponent<Image>();
    }

    public void Init(int x, int y, bool ans)
    {   
        this.row = x;
        this.col = y;
        this.ans = ans;
        SetImage(0);
    }

    public void Reset()
    {
        _state.setState(false, false, false);
        SetImage(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && _state.canPaint()) //좌클릭
        {
            if (ans)
            {
                SetImage(1);
                GetComponentInParent<Grid>().CorrectAnswer();
            }
            else
            {
                SetImage(2);
                _state._isWrong = true;
                GetComponentInParent<Grid>().MinusLife();
            }
            _state._isPainted = true;
        }
        else if (Input.GetMouseButton(1) && _state.canPaint()) //우클릭
        {
            SetImage(3);
            _state._isChecked = true;
        }
        else if (Input.GetMouseButton(1) && _state._isChecked) //우클릭
        {
            SetImage(0);
            _state._isChecked = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && _state.canPaint()) //좌클릭
        {
            if (ans)
            {
                SetImage(1);
                GetComponentInParent<Grid>().CorrectAnswer();
            }
            else
            {
                SetImage(2);
                _state._isWrong = true;
                GetComponentInParent<Grid>().MinusLife();
            }
            _state._isPainted = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _state.canPaint()) //우클릭
        {
            SetImage(3);
            _state._isChecked = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _state._isChecked) //우클릭
        {
            SetImage(0);
            _state._isChecked = false;
        }
    }

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