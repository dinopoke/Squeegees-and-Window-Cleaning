using System;
using System.Collections;
using UnityEngine;
using static GameManager;

public class Toilet : MonoBehaviour {

    [SerializeField] private PlayerStats playerStats;

    public static event Action<string> toiletText;

    private Coroutine goingToToilet;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform == transform) {
                    OnClick();
                }
            }
        }
    }

    private void OnClick() {
        if (playerStats.CheckToilet(50)) {
            toiletText?.Invoke("I don't need to use the toilet");
        } else {
            GameManager.Instance.currentGamestate = GameState.occupied;
            if (goingToToilet != null) StopCoroutine(goingToToilet);
            goingToToilet = StartCoroutine(UsingToilet());
        }
    }

    private IEnumerator UsingToilet() {
        while (!playerStats.CheckToilet(99)) {
            yield return new WaitForSeconds(.2f);
            toiletText?.Invoke("Using the Toilet...");
            playerStats.ChangeToilet(1);
        }
        GameManager.Instance.currentGamestate = GameState.takingBreak;
        toiletText?.Invoke("Finished!");
    }
}