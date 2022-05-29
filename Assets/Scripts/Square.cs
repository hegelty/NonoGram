using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Square : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public bool _isPainted = false; //색칠되어 있는지
    public bool _isChecked = false; //체크(아닌곳)되어 있는지
    public bool _isWrong = false; //오답인지
    public GameObject square;
    public Image image;
    public Sprite baseImage, ansImage, wrongImage, checkImage;

    public int row, col;
    public bool ans;
    void Start()
    {
        image = square.GetComponent<Image>();
    }

    void Update()
    {
        
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
        _isPainted = false;
        _isChecked = false;
        _isWrong = false;
        SetImage(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && _isChecked == false && _isWrong == false) //좌클릭
        {
            if (ans)
            {
                SetImage(1);
                GetComponentInParent<Grid>().CorrectAnswer();
            }
            else
            {
                SetImage(2);
                _isWrong = true;
                GetComponentInParent<Grid>().MinusLife();
            }
            _isPainted = true;
        }
        else if (Input.GetMouseButton(1) && _isPainted == false && _isChecked == false && _isWrong == false) //우클릭
        {
            SetImage(3);
            _isChecked = true;
        }
        else if (Input.GetMouseButton(1) && _isChecked == true) //우클릭
        {
            SetImage(0);
            _isChecked = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && _isChecked == false && _isWrong == false) //좌클릭
        {
            if (ans)
            {
                SetImage(1);
                GetComponentInParent<Grid>().CorrectAnswer();
            }
            else
            {
                SetImage(2);
                _isWrong = true;
                GetComponentInParent<Grid>().MinusLife();
            }
            _isPainted = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _isPainted == false && _isChecked == false && _isWrong == false) //우클릭
        {
            SetImage(3);
            _isChecked = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _isChecked == true) //우클릭
        {
            SetImage(0);
            _isChecked = false;
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