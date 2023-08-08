using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class EventsManager   
{
    //ı
    public static Action<int> onGenerateButtonClick;
    public static Action  onAllCellsAreInitialized;
    public static Action<int>  onMatchCount;

}
