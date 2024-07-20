using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LiftControl : MonoBehaviour
{
    private PlayerControls.Input controls;

    public CameraManager cameraManager;

    public WindowDetector windowDetector;

    public float speed = 10;
    private Vector2 moveInput;
    private Rigidbody rb;


    void Awake() {
        controls = new PlayerControls.Input();
        rb = this.GetComponent<Rigidbody>();
    }

    void OnEnable() {
        controls.Player.SwitchCamera.performed += SwapCamera;
        controls.Player.SwitchCamera.canceled += SwapCamera;
        controls.Player.LiftMove.performed += LiftMove;
        controls.Player.LiftMove.canceled += LiftMove;
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    void SwapCamera(InputAction.CallbackContext context) {
        if(context.performed){
            if(GameManager.Instance.canClean == true){
                cameraManager.SwapCameras();
                if(GameManager.Instance.currentGamestate == GameManager.GameState.movingLift){
                    GameManager.Instance.currentGamestate = GameManager.GameState.cleaning;
                    windowDetector.currentWindow.GetComponent<CleanableWindow>().enabled = true;
                }
                else if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning){
                    windowDetector.currentWindow.GetComponent<CleanableWindow>().enabled = false;
                    GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;

                }
            }

        }
    }

    void LiftMove(InputAction.CallbackContext context) {
        if (GameManager.Instance.currentGamestate == GameManager.GameState.movingLift){
            moveInput = context.ReadValue<Vector2>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveInput * speed * Time.fixedDeltaTime;
    }
}
