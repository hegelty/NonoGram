using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public float everySqaureOffset = 0.0f;
    public GameObject gridSquare;
    public TextMeshProUGUI lifeText;
    private List<GameObject> _gridSquares = new List<GameObject>();
    public Vector2 startPosition = new Vector2(-75f, -45f);

    public int mapSize;
    public int lifes;
    public bool[,] MapInfo;
    
    void Start()
    {
        LoadMapInfo(0);
        CreateGrid();
        SetLifeText(lifes);
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
    
    public void LoadMapInfo(int mapID)
    {
        FileStream fs = new FileStream("./Assets/Maps/map_" + mapID + ".txt", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        mapSize = Int32.Parse(sr.ReadLine());
        lifes = Int32.Parse(sr.ReadLine());
        MapInfo = new bool[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            String line = sr.ReadLine();
            for(int j=0;j<mapSize;j++)
            {
                if (line != null) MapInfo[i, j] = ((line[j] - '0') == 1);
                else MapInfo[i, j] = false;
            }
        }
    }
    
    public void MinusLife()
    {
        lifes--;
        SetLifeText(lifes);
        if (lifes == 0)
        {
            Debug.Log("Game Over");
        }
    }
    
    public void SetLifeText(int life)
    {
        lifeText.text = "Life : " + life;
    }
}