using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LiftControl : MonoBehaviour
{
    private PlayerControls.Input controls;

    public CameraManager cameraManager;

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
            cameraManager.SwapCameras();
        }
    }

    void LiftMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveInput * speed * Time.deltaTime;
    }
}
