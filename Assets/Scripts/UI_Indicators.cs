using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Indicators : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI noOfWindowsCleanText;


    private void Start() {
        GameManager.AddCleanedWindowEvent += OnCleanWindow;        
    }

    private void OnCleanWindow() {
        noOfWindowsCleanText.text = GameManager.Instance.NumberOfWindowsCleaned.ToString();
    }

    private void OnDisable() {
        GameManager.AddCleanedWindowEvent -= OnCleanWindow;
    }

}
