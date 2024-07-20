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
        if (windowCamera.activeSelf == true){
            windowCamera.SetActive(false);
            buildingCamera.SetActive(true);
        }
        else{
            windowCamera.SetActive(true);
            buildingCamera.SetActive(false);
        }
    }
}
