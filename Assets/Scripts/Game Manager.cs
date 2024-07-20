using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState {cleaning, movingLift};

    public bool canClean;

    public GameState currentGamestate;

    public CameraManager cameraManager;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
