using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GridInfoUpdater: MonoBehaviour {
    //ı 
    #region Fields
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button generateCellSizeButton;
    [SerializeField] TMP_Text matchCounterText;
    #endregion

    private void OnEnable()
    {
        EventsManager.onMatchCount += OnMatchCount;
    }


    private void OnDisable()
    {
        EventsManager.onMatchCount -= OnMatchCount;

    }
    private void OnMatchCount(int matchCount)
    {
        matchCounterText.text = matchCount.ToString();
    }
    public void OnGenerateClick()
    {
        int selectedValue = int.Parse(inputField.text);
        EventsManager.onGenerateButtonClick?.Invoke(selectedValue);
    }
    public void OnInputFieldValueChanged()
    {
        //check inputfield to register valid values
        if(inputField.text == string.Empty)
        {
            inputField.text = "0";
        }
        if(int.Parse(inputField.text) >= 2)
        {
            generateCellSizeButton.interactable = true;
        } else
        {
            generateCellSizeButton.interactable = false;

        }
    }
}
