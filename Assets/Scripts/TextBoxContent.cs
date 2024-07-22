using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxContent {

    private List<string> hungerStrings;
    private List<string> toiletStrings;

    public TextBoxContent() {
        SetupLists();
    }

    private void SetupLists() {
        hungerStrings = new();
        AddHungerString();
        toiletStrings = new();
        AddToiletString();
    }

    private void AddHungerString() {
        hungerStrings.Add("I'm too hungry to work!");
        hungerStrings.Add("I'm so hungry...");
        hungerStrings.Add("I need to eat before I can clean.");
        hungerStrings.Add("Food.. I need food..");
        hungerStrings.Add("Too hungry. I can't work anymore!");
    }
    
    private void AddToiletString() {
        toiletStrings.Add("I'm busting to use the Toilet!");
        toiletStrings.Add("I gotta go use the bathroom!");
        toiletStrings.Add("Ugh.. I need the restroom...");
        toiletStrings.Add("I need a toilet break first!");
        toiletStrings.Add("Nature calls! Work can wait!");
    }

    public string GetRandomHungerText() {
        return hungerStrings[Random.Range(1,hungerStrings.Count)];
    }

    public string GetRandomToiletText() {
        return toiletStrings[Random.Range(1,toiletStrings.Count)];
    }

}
