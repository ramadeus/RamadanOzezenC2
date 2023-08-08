using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell: MonoBehaviour {
    //ı
    #region Fields
    public bool isSelected;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite selectedSprite;
    SpriteRenderer spriteRenderer;
    public Vector2 gridMatrix;
    List<Cell> neighbors = new List<Cell>();
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        EventsManager.onAllCellsAreInitialized += InitializeNeighbors;
    }

    private void OnDisable()
    {
        EventsManager.onAllCellsAreInitialized -= InitializeNeighbors;

    } 
    
    private void OnMouseDown()
    {
        SelectCell(true);
        MatchHandler.instance.cellsToUnselect.Clear(); // every click resets the selection path.
        MatchHandler.instance.cellsToUnselect.Add(this); // the first member of the path should be the clicked cell
        GetNeighborsPaths(); // Send the news to the neighbors and check the selection path
        StartCoroutine(WaitForLastCellDrawThenCheckTheSelections()); // to get smoother view
   
    }
    IEnumerator WaitForLastCellDrawThenCheckTheSelections()
    {
        yield return new WaitForSecondsRealtime(.1f);
        MatchHandler.instance.CheckSelections();

    }

    private void GetNeighborsPaths(Cell requesterCell = default)
    { 
        for(int i = 0; i < neighbors.Count; i++)
        {   // IMPORTANT: if the neighbor's neighbor is us, it will skip this step so we don't get stack overflow.
            if(neighbors[i] == requesterCell)
            {
                continue;
            }
            if(neighbors[i].isSelected)
            { 
                MatchHandler.instance.cellsToUnselect.Add(neighbors[i]);
                neighbors[i].GetNeighborsPaths(this);
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
    public void UnselectThisCell()
    {
        SelectCell(false); 
    }

    public void InitializeNeighbors(List<Cell> _neighbors)
    {
        neighbors = _neighbors;
    }
    #region Initializers
    private void InitializeNeighbors()
    {
        List<Cell> allCells = GridManager.instance.cells;

        List<Vector2> possibleGridPositions = GetPossibleNeighborPositions();

        for(int i = 0; i < allCells.Count; i++)
        {
            for(int j = 0; j < possibleGridPositions.Count; j++)
            {
                // find the matched neighbor gridMatrixes so we can know it's cell's neighbor 
                if(allCells[i].gridMatrix == possibleGridPositions[j])
                {
                    neighbors.Add(allCells[i]);
                }
            }
        }
    }
    /// <summary>
    /// Algorithm: Every cell checks for the four of the directions by increasing and decreasing (by 1) their column and row values. 
    /// If the direction is available, it adds the neighbor cell to its data.
    /// This method called one time.
    /// </summary>
    /// <returns></returns>
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
    #endregion


}
