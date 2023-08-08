using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridManager: MonoBehaviour {
    //ı
    #region Fields
    public int cellCount = 3;
    public List<Cell> cells;
    public static GridManager instance;

    [SerializeField] GameObject cellPrefab;

    private int columns;
    private int rows;
    private float cellSize = 1;
    #endregion
    #region Manager Methods
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        } 
    }
    private void Start()
    {
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
    #endregion 
    #region Initializers
    private void ClearLastCells()
    {
        transform.position = Vector2.zero;
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    void GenerateCells()
    {
        ClearLastCells();
        cellSize = Camera.main.orthographicSize / cellCount;
        columns = rows = cellCount;
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                SpawnGridCell(i, j);
            }
        }
        EventsManager.onAllCellsAreInitialized?.Invoke();
        SetGridAreaPoisiton();

    } 
    private void SetGridAreaPoisiton()
    {
        Transform lastGridCell = transform.GetChild(transform.childCount - 1);
        float xPos = lastGridCell.localPosition.x / 2f;
        float yPos = lastGridCell.localPosition.y / 2f;

        transform.position = new Vector2(-xPos, -yPos + 2);
    }


    private void SpawnGridCell(int x, int y)
    {

        GameObject gridCell = Instantiate(cellPrefab, transform);
        gridCell.name = "Cell X:" + x + "Y:" + y;

        float scale = cellSize - (cellSize * .1f); // to get the cell smoother view
        gridCell.transform.localScale = new Vector2(scale, scale);
        gridCell.transform.position = new Vector3(x * cellSize, -y * cellSize, 0);
        Cell cellScript = gridCell.GetComponent<Cell>();
        cellScript.gridMatrix = new Vector2(x, y);
        cells.Add(cellScript);
    }
    #endregion
}
