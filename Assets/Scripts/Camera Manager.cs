using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject windowCamera;
    public GameObject buildingCamera;

    private float windowCamZPos;

    public Outline currentOutline;

    // Start is called before the first frame update
    void Start()
    {
        windowCamZPos = windowCamera.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWindowCameraPosition(Vector2 windowPosition) {
        windowCamera.transform.position = new(windowPosition.x, windowPosition.y, windowCamZPos);
    }


    public void SwapCameras(){
        if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning){
            windowCamera.SetActive(false);
            buildingCamera.SetActive(true);
            if (currentOutline != null) currentOutline.enabled = true;
        }
        else if(GameManager.Instance.currentGamestate == GameManager.GameState.movingLift){
            windowCamera.SetActive(true);
            buildingCamera.SetActive(false);
            if (currentOutline != null) currentOutline.enabled = false;
        }
    }
}
