using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private int hunger = 100;
    private int toiletBreak = 100;

    [SerializeField] private float hungerRate = 4f;
    [SerializeField] private float toiletRate = 10f;
    private float hungerUrgencyRate = 0f;
    private float toiletUrgencyRate = 0f;

    private float hungerCounter = -15f;
    private float toiletCounter = -25f;

    public static event Action<int> OnHungerChange;
    public static event Action<int> OnToiletChange;

    private void Update() {

        hungerCounter += Time.deltaTime;
        toiletCounter += Time.deltaTime;

        if (hungerCounter > (hungerRate - hungerUrgencyRate)) {
            hungerCounter = 0f;
            if (hunger >= 0) hunger--;
            if (hunger < 25) {
                hungerUrgencyRate = 2f;
            } else if (hunger < 50) {
                hungerUrgencyRate = 1f;
            }
            OnHungerChange?.Invoke(hunger);
        }

        if (toiletCounter > (toiletRate - hungerUrgencyRate)) {
            toiletCounter = 0f;
            if (toiletBreak >= 0) toiletBreak--;
            if (toiletBreak < 25) {
                toiletUrgencyRate = 2f;
            } else if (toiletBreak < 50) {
                toiletUrgencyRate = 1f;
            }
            OnToiletChange?.Invoke(toiletBreak);
        }
    }


}

