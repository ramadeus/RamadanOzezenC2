using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell: MonoBehaviour {
    //ı
    public bool isSelected;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite selectedSprite;
    SpriteRenderer spriteRenderer;
    public Vector2 gridMatrix;
    List<Cell> neighbours = new List<Cell>(); 
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EventsManager.onAllCellsAreInitialized += GenerateNeighbors;
    }

    private void OnDisable()
    {
        EventsManager.onAllCellsAreInitialized -= GenerateNeighbors;

    }

    private void GenerateNeighbors()
    {
        List<Cell> cells = GridManager.instance.cells;

        List<Vector2> possibleGridPositions = GetPossibleNeighborPositions();

        for(int i = 0; i < cells.Count; i++)
        {
            for(int j = 0; j < possibleGridPositions.Count; j++)
            {
                if(cells[i].gridMatrix == possibleGridPositions[j])
                {
                    neighbours.Add(cells[i]);
                }
            }
        }
    }


    private void OnMouseDown()
    {
        SelectCell(true);
        MatchHandler.instance.cellsToUnselect.Clear();
        MatchHandler.instance.cellsToUnselect.Add(this);
        CheckNeighbors();
        StartCoroutine(WaitForLastCellDraw());
   
    }
    IEnumerator WaitForLastCellDraw()
    {
        yield return new WaitForSecondsRealtime(.1f);
        MatchHandler.instance.CheckSelections();

    }

    private void CheckNeighbors(Cell requesterCell = default)
    { 
        for(int i = 0; i < neighbours.Count; i++)
        {
            if(neighbours[i] == requesterCell)
            {
                continue;
            }
            if(neighbours[i].isSelected)
            { 
                MatchHandler.instance.cellsToUnselect.Add(neighbours[i]);
                neighbours[i].CheckNeighbors(this);
            }
        }

      


    } 
    private void SelectCell(bool stat)
    {
        if(stat)
        {
            isSelected = true;
            spriteRenderer.sprite = selectedSprite; 
        } else
        {
            isSelected = false;
            spriteRenderer.sprite = emptySprite;
        }
    }
    private List<Vector2> GetPossibleNeighborPositions()
    {
        List<Vector2> gridPositions = new List<Vector2>();
        int cellCount = GridManager.instance.cellCount;
        bool up = (int)gridMatrix.y + 1 < cellCount;
        bool down = (int)gridMatrix.y - 1 >= 0;
        bool right = (int)gridMatrix.x + 1 < cellCount;
        bool left = (int)gridMatrix.x - 1 >= 0;
        if(up)
        {
            gridPositions.Add(new Vector2(gridMatrix.x, gridMatrix.y + 1));
        }
        if(down)
        {
            gridPositions.Add(new Vector2(gridMatrix.x, gridMatrix.y - 1));
        }
        if(right)
        {
            gridPositions.Add(new Vector2(gridMatrix.x + 1, gridMatrix.y));
        }
        if(left)
        {
            gridPositions.Add(new Vector2(gridMatrix.x - 1, gridMatrix.y));
        }
        return gridPositions;
    }

    public void UnselectThisCell()
    {
        SelectCell(false); 
    }

    public void InitializeNeighbors(List<Cell> _neighbors)
    {
        neighbours = _neighbors;
    }


}
