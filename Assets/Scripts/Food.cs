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
        } else if (!playerStats.CheckToilet(10)) {
            lunchText?.Invoke("I should use the toilet first!");
        } else {
            GameManager.Instance.currentGamestate = GameState.occupied;
            if (eating != null) StopCoroutine(eating);
            eating = StartCoroutine(EatingLunch());
            MusicPlayer.instance.playJingle1();
            AudioManager.PlaySound(AudioManager.Sound.eat, this.transform.position);
        }
    }

    private IEnumerator EatingLunch() {
        while (!playerStats.CheckHunger(99)) {
            yield return new WaitForSeconds(.2f);
            lunchText?.Invoke("Eating Food...");
            playerStats.ChangeHunger(2);
            //playerStats.ChangeToilet(-1);
        }
        GameManager.Instance.currentGamestate = GameState.takingBreak;
        playerStats.ChangeHungerRate(1.3f);
        if (!playerStats.CheckToilet(90)) playerStats.Fullness(true);
        lunchText?.Invoke("Done!");
    }
}