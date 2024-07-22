using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Indicators : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI noOfWindowsCleanText;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider toiletSlider;
    [SerializeField] private Image hungerColour;
    [SerializeField] private Image toiletColour;

    [SerializeField] private GameObject textPopUpBox;
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private float textBoxTime = 4f;
    private Coroutine showTextBox;

    private void Start() {
        GameManager.AddCleanedWindowEvent += OnCleanWindow;
        PlayerStats.OnHungerChange += OnHungerChange;
        PlayerStats.OnToiletChange += OnToiletChange;
        LiftControl.sendTextPopUp += ShowPopUpText;
        CleanableWindow.sendTextPopUp += ShowPopUpText;
    }

    private void OnCleanWindow() {
        noOfWindowsCleanText.text = GameManager.Instance.NumberOfWindowsCleaned.ToString();
    }

    private void OnHungerChange(int hunger) {
        hungerSlider.value = hunger;
        if (hunger < 25) {
            hungerColour.color = Color.red;
        } else if (hunger < 50) {
            hungerColour.color = Color.yellow;
        } else {
            hungerColour.color = Color.green;
        }
    }

    private void OnToiletChange(int toilet) {
        toiletSlider.value = toilet;
        if (toilet < 25) {
            toiletColour.color = Color.red;
        } else if (toilet < 50) {
            toiletColour.color = Color.yellow;
        } else {
            toiletColour.color = Color.green;
        }
    }

    private void ShowPopUpText(string text) {
        popUpText.text = text;
        if (showTextBox != null) StopCoroutine(showTextBox);
        textPopUpBox.SetActive(true);
        showTextBox = StartCoroutine(TextBoxAliveTime());
    }

    private IEnumerator TextBoxAliveTime() {
        yield return new WaitForSeconds(textBoxTime);
        textPopUpBox.SetActive(false);
    }

    private void OnDisable() {
        GameManager.AddCleanedWindowEvent -= OnCleanWindow;
        PlayerStats.OnHungerChange -= OnHungerChange;
        PlayerStats.OnToiletChange -= OnToiletChange;
        LiftControl.sendTextPopUp -= ShowPopUpText;
        CleanableWindow.sendTextPopUp -= ShowPopUpText;
    }

}
