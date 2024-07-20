using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject windowCamera;
    public GameObject buildingCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapCameras(){
        if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning){
            windowCamera.SetActive(false);
            buildingCamera.SetActive(true);
        }
        else if(GameManager.Instance.currentGamestate == GameManager.GameState.movingLift){
            windowCamera.SetActive(true);
            buildingCamera.SetActive(false);
        }
    }
}
