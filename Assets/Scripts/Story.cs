using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Story : MonoBehaviour, IPointerDownHandler
{
    public GameObject backGroundPrefeb;
    public GameObject storyTextPrefeb;
    private GameObject backGround;
    private GameObject storyText;
    private int _storyId = -1;
    private string[] _storyData;
    private int _currentLine = 0;
    private TextMeshProUGUI _text;

    public void ShowStory(int storyId)
    {
        var widthRatio = Screen.width / 540;
        _storyId = storyId;
        backGround = Instantiate(backGroundPrefeb, this.transform, true);
        backGround.transform.localPosition = new Vector3(0, 0, 0);
        backGround.GetComponent<Image>().color = new Color(0, 0, 0, 180f/255f);
        storyText = Instantiate(storyTextPrefeb, this.transform, true);
        storyText.GetComponent<TextMeshProUGUI>().fontSize = 24 * widthRatio;
        storyText.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.8f, storyText.GetComponent<RectTransform>().rect.height);
        storyText.transform.localPosition = new Vector3(0,10000/widthRatio,0);
        _text = storyText.GetComponent<TextMeshProUGUI>();
        _text.text = "";
        LoadStory(storyId);
    }

    private void LoadStory(int storyId)
    {
        TextAsset storyFile = Resources.Load<TextAsset>("Stories/story_" + storyId);
        var sr = new StringReader(storyFile.text);
        _storyData = sr.ReadToEnd().Split("\n");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && _storyId >= 0) //좌클릭
        {
            if(_currentLine>=_storyData.Length)
            {
                Destroy(storyText);
                Destroy(backGround);
                _storyId = -1;
                GetComponentInParent<GameManager>().ReadStory();
            }
            else
            {
                _text.text += "\n\n" + _storyData[_currentLine++];
            }
        }
    }
}
