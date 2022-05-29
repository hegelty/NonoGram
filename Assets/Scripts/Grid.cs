using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    public float everySqaureOffset = 0.0f;
    public GameObject gridSquare;
    public GameObject horizontalHintText;
    public GameObject verticalHintText;
    public TextMeshProUGUI lifeText;
    private List<GameObject> _gridSquares = new List<GameObject>();
    public Vector2 startPosition = new Vector2(-75f, -45f);
    public Vector2 HorizontalPosition = new Vector2(-120f, 163f);
    public Vector2 VerticalPosition = new Vector2(-75f, 210f);
    public float hintTextOffset = 23f;
    public int map_Id = 0;
    
    public int mapSize;
    public int lifes;
    public int leftAnswers = 0;
    public bool[,] MapInfo;
    
    void Start()
    {
        LoadMapInfo(map_Id);
        CreateGrid();
        SetLifeText(lifes);
        SpawnHintText();
    }

    private void CreateGrid()
    {
        SpawnGridsquare();
        SetSquarePosition();
    }
    

    private void SpawnGridsquare()
    {
        // 1, 2, 3, 4, 5, 6
        // 7, 8, 9, 10, ...
        for (int row = mapSize-1; row >= 0; row--)
        {
            for (int col = 0; col < mapSize; col++)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.parent = this.transform; //인스턴스를 오브젝트의 자식으로
                _gridSquares[_gridSquares.Count - 1].name = "Grid_" + row + "_" + col;
                _gridSquares[_gridSquares.Count - 1].GetComponent<Square>().Init(row, col, MapInfo[row, col]);
            }
        }
    }

    private void SetSquarePosition()
    {
        var sqaureRect = _gridSquares[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = sqaureRect.rect.width * sqaureRect.transform.localScale.x + everySqaureOffset;
        offset.y = sqaureRect.rect.height * sqaureRect.transform.localScale.y + everySqaureOffset;

        int colNum = 0, rowNum = 0;
        foreach (GameObject square in _gridSquares)
        {
            if (colNum == mapSize)
            {
                colNum = 0;
                rowNum++;
            }

            var posXOffset = offset.x * colNum;
            var posYOffset = offset.y * rowNum;

            square.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(startPosition.x + posXOffset, startPosition.y + posYOffset);
            colNum++;
        }
    }

    public void SpawnHintText()
    {
        for (int i = 0; i < mapSize; i++)
        {
            var hintText = Instantiate(horizontalHintText);
            hintText.transform.parent = this.transform;
            hintText.name = "HorizontalHintText_" + i;
            hintText.GetComponent<TextMeshProUGUI>().text = "";
            hintText.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(HorizontalPosition.x, HorizontalPosition.y - hintTextOffset*i);

            int t = 0;
            for (int j = 0; j < mapSize; j++)
            {
                if (t!=0&&!MapInfo[i, j])
                {
                    hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
                    t = 0;
                }
                else if (MapInfo[i, j]) t++;
            }
            if(t!=0) hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
        }
        for (int i = 0; i < mapSize; i++)
        {
            var hintText = Instantiate(verticalHintText);
            hintText.transform.parent = this.transform;
            hintText.name = "VerticalHintText_" + i;
            hintText.GetComponent<TextMeshProUGUI>().text = "";
            hintText.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(VerticalPosition.x + hintTextOffset * i, VerticalPosition.y);

            int t = 0;
            for (int j = 0; j < mapSize; j++)
            {
                if (t!=0&&!MapInfo[j, i])
                {
                    hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
                    t = 0;
                }
                else if (MapInfo[j, i]) t++;
            }
            if(t!=0) hintText.GetComponent<TextMeshProUGUI>().text += " " + (t);
        }
    }
    
    public void LoadMapInfo(int mapID)
    {
        FileStream fs = new FileStream("./Assets/Maps/bin_" + mapID + ".txt", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        mapSize = Int32.Parse(sr.ReadLine());
        lifes = Int32.Parse(sr.ReadLine());
        leftAnswers = 0;
        MapInfo = new bool[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            String line = sr.ReadLine();
            for(int j=0;j<mapSize;j++)
            {
                if (line != null) MapInfo[i, j] = ((line[j] - '0') == 1);
                else MapInfo[i, j] = false;
                leftAnswers+=MapInfo[i, j] ? 1 : 0;
            }
        }
    }
    
    public void MinusLife()
    {
        lifes--;
        SetLifeText(lifes);
        if (lifes <= 0)
        {
            ResetMap();
        }
    }
    
    public void CorrectAnswer()
    {
        leftAnswers--;
        if (leftAnswers == 0)
        {
            ClearMap();
        }
    }
    
    public void SetLifeText(int life)
    {
        lifeText.text = "Life : " + life;
    }

    public void ResetMap()
    {
        foreach (GameObject square in _gridSquares)
        {
            square.GetComponent<Square>().Reset();
        }
        LoadMapInfo(map_Id);
        SetLifeText(lifes);
    }

    public void ClearMap()
    {
        foreach (GameObject square in _gridSquares)
        {
            Destroy(gameObject);
            lifeText.text = "";
        }
    }
}