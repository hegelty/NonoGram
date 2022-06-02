using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Int32;

public class Grid : MonoBehaviour
{
    public float everySquareOffset;
    public GameObject gridSquare;
    public GameObject horizontalHintText;
    public GameObject verticalHintText;
    public TextMeshProUGUI lifeText;
    private List<GameObject> _gridSquares = new List<GameObject>();
    private List<GameObject> _horizontalHintTexts, _verticalHintTexts;
    public Vector2 startPosition, horizontalPosition, verticalPosition;
    public float hintTextOffset;

    private int _mapId;
    
    private int _mapSize, _lifes, _leftAnswers;
    private bool[,] _mapInfo;

    public void StartGame(int mapId)
    {
        _mapId = mapId;
        if(mapId!=1) DestroyMap();
        LoadMapInfo(mapId);
        CreateGrid();
        SetLifeText(_lifes);
        SpawnHintText();
    }

    private void CreateGrid()
    {
        SpawnGridSquare();
        SetSquarePosition();
    }
    

    private void SpawnGridSquare()
    {
        _gridSquares = new List<GameObject>();
        for (int row = _mapSize-1; row >= 0; row--)
        {
            for (int col = 0; col < _mapSize; col++)
            {
                _gridSquares.Add(Instantiate(gridSquare));
                _gridSquares[_gridSquares.Count - 1].transform.parent = this.transform; //인스턴스를 오브젝트의 자식으로
                _gridSquares[_gridSquares.Count - 1].name = "Grid_" + row + "_" + col;
                _gridSquares[_gridSquares.Count - 1].GetComponent<Square>().Init(row, col, _mapInfo[row, col]);
            }
        }
    }

    private void SetSquarePosition()
    {
        var squareRect = _gridSquares[0].GetComponent<RectTransform>();
        var offset = new Vector2();
        var rect = squareRect.rect;
        var widthRatio = Screen.width / 540;
        var localScale = squareRect.transform.localScale;
        offset.x = (rect.width * localScale.x + everySquareOffset) * widthRatio;
        offset.y = (rect.height * localScale.y + everySquareOffset) * widthRatio;
        var offsetR = (widthRatio - 1) * rect.width;
        int colNum = 0, rowNum = 0;
        foreach (var square in _gridSquares)
        {
            if (colNum == _mapSize)
            {
                colNum = 0;
                rowNum++;
            }

            var posXOffset = offset.x * colNum;
            var posYOffset = offset.y * rowNum;
            square.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(startPosition.x - offsetR + posXOffset, startPosition.y - offsetR + posYOffset);
            square.GetComponent<RectTransform>().localScale *= widthRatio;
            colNum++;
        }
    }

    private void SpawnHintText()
    {
        _horizontalHintTexts = new List<GameObject>();
        _verticalHintTexts = new List<GameObject>();
        var squareRect = _gridSquares[0].GetComponent<RectTransform>();
        var rect = squareRect.rect;
        var widthRatio = Screen.width / 540;
        var offsetR = (widthRatio - 1) * rect.width;
        hintTextOffset += everySquareOffset * (widthRatio - 1);
        
        for (var i = 0; i < _mapSize; i++)
        {
            var hintText = Instantiate(horizontalHintText, this.transform, true);
            _horizontalHintTexts.Add(hintText);
            hintText.name = "HorizontalHintText_" + i;
            hintText.GetComponent<TextMeshProUGUI>().fontSize *= widthRatio;
            hintText.GetComponent<RectTransform>().sizeDelta = new Vector2(120 * widthRatio, 25 * widthRatio);
            hintText.GetComponent<TextMeshProUGUI>().text = "";
            hintText.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(horizontalPosition.x - offsetR, horizontalPosition.y - hintTextOffset * i);

            var t = 0;
            for (var j = 0; j < _mapSize; j++)
            {
                if (t!=0&&!_mapInfo[i, j])
                {
                    hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
                    t = 0;
                }
                else if (_mapInfo[i, j]) t++;
            }
            if(t!=0) hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
        }
        for (var i = 0; i < _mapSize; i++)
        {
            var hintText = Instantiate(verticalHintText, this.transform, true);
            _verticalHintTexts.Add(hintText);
            hintText.name = "VerticalHintText_" + i;
            hintText.GetComponent<TextMeshProUGUI>().text = "";
            hintText.GetComponent<TextMeshProUGUI>().fontSize *= widthRatio;
            hintText.GetComponent<RectTransform>().sizeDelta = new Vector2(25 * widthRatio, 120 * widthRatio);
            hintText.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(verticalPosition.x - offsetR + hintTextOffset * i, verticalPosition.y);

            var t = 0;
            for (var j = 0; j < _mapSize; j++)
            {
                if (t!=0&&!_mapInfo[j, i])
                {
                    hintText.GetComponent<TextMeshProUGUI>().text += "\n" + (t);
                    t = 0;
                }
                else if (_mapInfo[j, i]) t++;
            }
            if(t!=0) hintText.GetComponent<TextMeshProUGUI>().text += "\n" + (t);
        }
    }

    private void LoadMapInfo(int mapID)
    {
        var fs = new FileStream("./Assets/Maps/bin_" + mapID + ".txt", FileMode.Open);
        var sr = new StreamReader(fs);
        _mapSize = Parse(sr.ReadLine() ?? string.Empty);
        _lifes = Parse(sr.ReadLine() ?? string.Empty);
        _leftAnswers = 0;
        _mapInfo = new bool[_mapSize, _mapSize];
        for (var i = 0; i < _mapSize; i++)
        {
            var line = sr.ReadLine();
            for(var j=0;j<_mapSize;j++)
            {
                if (line != null) _mapInfo[i, j] = ((line[j] - '0') == 1);
                else _mapInfo[i, j] = false;
                _leftAnswers+=_mapInfo[i, j] ? 1 : 0;
            }
        }
        fs.Close();
    }
    
    public void MinusLife()
    {
        _lifes--;
        SetLifeText(_lifes);
        if (_lifes <= 0)
        {
            ResetMap();
        }
    }
    
    public void CorrectAnswer()
    {
        _leftAnswers--;
        if (_leftAnswers == 0)
        {
            GetComponentInParent<GameManager>().ClearStage();
        }
    }

    private void SetLifeText(int life)
    {
        lifeText.text = "Life : " + life;
    }

    private void ResetMap()
    {
        foreach (GameObject square in _gridSquares)
        {
            square.GetComponent<Square>().Reset();
        }
        LoadMapInfo(_mapId);
        SetLifeText(_lifes);
    }

    private void DestroyMap()
    {
        foreach (GameObject square in _gridSquares)
        {
            Destroy(square);
        }
        foreach (GameObject hintText in _verticalHintTexts)
        {
            Destroy(hintText);
        }
        foreach (GameObject hintText in _horizontalHintTexts)
        {
            Destroy(hintText);
        }
    }
}