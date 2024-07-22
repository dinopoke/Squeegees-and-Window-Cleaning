using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState {cleaning, movingLift, takingBreak, menu, occupied};

    public bool canClean;

    public bool canTakeBreak;

    public GameState currentGamestate;

    public CameraManager cameraManager;

    public GameObject cleanWindow;

    public int NumberOfWindowsCleaned;
    public static event Action AddCleanedWindowEvent;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        currentGamestate = GameState.movingLift;
        canClean = false;
        canTakeBreak = false;
    }

    public void ReplaceWindowWithClean(Vector3 position) {
        Instantiate(cleanWindow, position, Quaternion.Euler(0, 90, 0));
    }

    public void AddCleanWindow() {
        NumberOfWindowsCleaned++;
        AddCleanedWindowEvent?.Invoke();
    }
}
