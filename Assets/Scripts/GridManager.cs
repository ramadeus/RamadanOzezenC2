using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridManager: MonoBehaviour {
    //ı

    public int cellCount = 3;
    public List<Cell> cells;
    public static GridManager instance;

    [SerializeField] GameObject cellPrefab;

    int columns;
    int rows;
    float tileSize = 1;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        GenerateCells();
    } 
    private void OnEnable()
    {
        EventsManager.onGenerateButtonClick += UpdateCells;
    }
    private void OnDisable()
    {
        EventsManager.onGenerateButtonClick -= UpdateCells;
    }

    private void UpdateCells(int value)
    {
        cellCount = value;
        GenerateCells();
    }



    void GenerateCells()
    {
        ClearLastCells();
        tileSize = Camera.main.orthographicSize / cellCount;
        columns = rows = cellCount;
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                SpawnGridCell(i, j);
            }
        }
        EventsManager.onAllCellsAreInitialized?.Invoke();
        SetGridPoisiton();

    }

    

    private void SetGridPoisiton()
    {
        Transform lastGridCell = transform.GetChild(transform.childCount - 1);
        float xPos = lastGridCell.localPosition.x / 2f;
        float yPos = lastGridCell.localPosition.y / 2f;

        transform.position = new Vector2(-xPos, -yPos + 2);
    }

    private void ClearLastCells()
    {
        transform.position = Vector2.zero;
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void SpawnGridCell(int x, int y)
    {

        GameObject gridCell = Instantiate(cellPrefab, transform);
        gridCell.name = "Cell X:" + x + "Y:" + y;
        gridCell.transform.localScale = new Vector2(tileSize - (tileSize * .1f), tileSize - (tileSize * .1f));
        gridCell.transform.position = new Vector3(x * tileSize, -y * tileSize, 0);
        Cell cellScript = gridCell.GetComponent<Cell>();
        cellScript.gridMatrix = new Vector2(x, y);
        cells.Add(cellScript);
    }
}
