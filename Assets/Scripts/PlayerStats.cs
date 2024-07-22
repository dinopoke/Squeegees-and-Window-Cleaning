using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private int hunger = 90;
    private int toiletBreak = 100;

    [SerializeField] private float hungerRate = 3f;
    [SerializeField] private float maxHungerRate = .5f;
    [SerializeField] private float toiletRate = 7f;
    private float hungerUrgencyRate = 0f;
    private float toiletUrgencyRate = 0f;

    private float hungerRateCalculation;
    private bool justAte;

    private float hungerCounter = -15f;
    private float toiletCounter = -25f;

    public static event Action<int> OnHungerChange;
    public static event Action<int> OnToiletChange;

    private void Start() {
        OnHungerChange?.Invoke(hunger);
        OnToiletChange?.Invoke(toiletBreak);
    }

    private void Update() {

        hungerCounter += Time.deltaTime;
        toiletCounter += Time.deltaTime;

        hungerRateCalculation = Mathf.Clamp(hungerRate - hungerUrgencyRate, maxHungerRate, 4f);

        if (hungerCounter > hungerRateCalculation) {
            hungerCounter = 0f;
            if (hunger >= 0) hunger--;
            if (hunger < 25) {
                hungerUrgencyRate = 1f;
            } else if (hunger < 60) {
                hungerUrgencyRate = .5f;
            } else if (hunger >= 60) {
                hungerUrgencyRate = 0f;
            }
            OnHungerChange?.Invoke(hunger);
        }

        if (toiletCounter > (toiletRate - toiletUrgencyRate - (justAte? 3.5 : 0))) {
            Debug.Log("Toilet Counter: " + toiletCounter);
            toiletCounter = 0f;
            if (toiletBreak >= 0) toiletBreak--;
            if (toiletBreak < 40) {
                toiletUrgencyRate = 3f;
            } else if (toiletBreak < 80) {
                toiletUrgencyRate = 2f;
            } else if (toiletBreak >= 80) {
                toiletUrgencyRate = 0f;
        }
        OnToiletChange?.Invoke(toiletBreak);
        }
    }

    public bool CheckHunger(int value) { 
        return hunger >= value;
    }

    public bool CheckToilet(int value) {
        return toiletBreak >= value;
    }

    public void ChangeHunger(int value) {
        int newHunger = hunger + value;
        hunger = (int)Mathf.Clamp(newHunger, 0f, 100f);
        OnHungerChange?.Invoke(hunger);
        hungerCounter = -24f;
    }
    
    public void ChangeToilet(int value) {
        int newToilet = toiletBreak + value;
        toiletBreak = (int)Mathf.Clamp(newToilet, 0f, 100f);
        OnToiletChange?.Invoke(toiletBreak);
        toiletCounter = -20f;
    }

    public void ChangeHungerRate(float value) {
        float newHungerRate = hungerRate + value;
        hungerRate = Mathf.Clamp(newHungerRate, maxHungerRate, 4f);
        Debug.Log("New Hunger Rate: " +  hungerRate);
    }

    public void Fullness(bool full) {
        justAte = full;
    }

}

