using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    //ı
    public List<Cell> cellsToUnselect =new();
    public static MatchHandler instance;
    int matchCount;
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

    public void CheckSelections()
    {
        if(cellsToUnselect.Count >=3)
        {  matchCount++;
            EventsManager.onMatchCount?.Invoke(matchCount);
            for(int i = 0; i < cellsToUnselect.Count; i++)
            {
                cellsToUnselect[i].UnselectThisCell();
            } 
          

        }
    }
}
