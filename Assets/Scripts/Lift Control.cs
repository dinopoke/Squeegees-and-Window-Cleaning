using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LiftControl : MonoBehaviour
{
    private PlayerControls.Input controls;

    public CameraManager cameraManager;

    void Awake() {
        controls = new PlayerControls.Input();
    }

    void OnEnable() {
        controls.Player.SwitchCamera.performed += SwapCamera;
        controls.Player.SwitchCamera.canceled += SwapCamera;
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    void SwapCamera(InputAction.CallbackContext context) {
        if(context.performed){
            cameraManager.SwapCameras();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
