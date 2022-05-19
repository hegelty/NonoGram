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
    public GameObject square;
    public Image image;

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
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && _isChecked == false) //좌클릭
        {
            if (ans)
                image.color = new Color32(0, 0, 0, 255);
            else
            {
                image.color = new Color32(255, 0, 0, 255);
                GetComponentInParent<Grid>().MinusLife();
            }
            _isPainted = true;
        }
        else if (Input.GetMouseButton(1) && _isPainted == false && _isChecked == false) //우클릭
        {
            image.color = new Color32(255, 155, 155, 255);
            _isChecked = true;
        }
        else if (Input.GetMouseButton(1)) //우클릭
        {
            image.color = new Color32(255, 255, 255, 255);
            _isChecked = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && _isChecked == false) //좌클릭
        {
            if (ans) 
                image.color = new Color32(0, 0, 0, 255);
            else
            {
                image.color = new Color32(255, 0, 0, 255);
                GetComponentInParent<Grid>().MinusLife();
            }
            _isPainted = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right && _isPainted == false && _isChecked == false) //우클릭
        {
            image.color = new Color32(255, 155, 155, 255);
            _isChecked = true;
        }
        else if (eventData.button == PointerEventData.InputButton.Right) //우클릭
        {
            Debug.Log("was checked");
            image.color = new Color32(255, 255, 255, 255);
            _isChecked = false;
        }
    }
}