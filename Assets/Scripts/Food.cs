using System;
using System.Collections;
using UnityEngine;
using static GameManager;

public class Food : MonoBehaviour {

    [SerializeField] private PlayerStats playerStats;

    public static event Action<string> lunchText;

    private Coroutine eating;

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
        if (playerStats.CheckHunger(50)) {
            lunchText?.Invoke("I'm not really that hungry");
        } else {
            GameManager.Instance.currentGamestate = GameState.occupied;
            if (eating != null) StopCoroutine(eating);
            eating = StartCoroutine(EatingLunch());
        }
    }

    private IEnumerator EatingLunch() {
        while (!playerStats.CheckHunger(99)) {
            yield return new WaitForSeconds(.2f);
            lunchText?.Invoke("Eating Food...");
            playerStats.ChangeHunger(2);
            playerStats.ChangeToilet(-1);
        }
        GameManager.Instance.currentGamestate = GameState.takingBreak;
        lunchText?.Invoke("Done!");
    }
}