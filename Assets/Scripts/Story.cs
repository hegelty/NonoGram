using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Story : MonoBehaviour, IPointerEnterHandler
{
    public GameObject backGround;
    public GameObject storyText;
    public float fadeTime;
    private int _storyId = -1;
    private string[] _storyData;
    private int _currentLine = 0;
    private TextMeshProUGUI _text;
    // Start is called before the first frame update
    
    void Start()
    {
    }
    
    public void ShowStory(int storyId)
    {
        backGround.GetComponent<Image>().color = new Color(0, 0, 0, 180f/255f);
        _storyId = storyId;
        _text = storyText.GetComponent<TextMeshProUGUI>();
        _text.text = "";
        LoadStory(storyId);
    }

    private void LoadStory(int storyId)
    {
        var fs = new FileStream("./Assets/Stories/story_" + storyId + ".txt", FileMode.Open);
        var sr = new StreamReader(fs);
        _storyData = sr.ReadToEnd().Split("\n");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && _storyId >= 0) //좌클릭
        {
            if(_currentLine>=_storyData.Length)
            {
                backGround.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                storyText.GetComponent<TextMeshProUGUI>().text = "";
                _storyId = 0;
                GetComponentInParent<GameManager>().ReadStory();
            }
            _text.text += "\n" + _storyData[++_currentLine];
        }
    }
}
