using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LiftControl : MonoBehaviour
{
    private PlayerControls.Input controls;
    [SerializeField] private PlayerStats playerStats;
    private TextBoxContent textBoxContent;

    public static event Action<string> sendTextPopUp;

    public CameraManager cameraManager;

    public WindowDetector windowDetector;

    public float speed = 10;
    public float downwardSpeedMultiplier = 2f;
    private Vector2 moveInput;
    private Rigidbody rb;

    private MeshRenderer meshRenderer;

    [SerializeField] private GameObject liftVisuals;


    void Awake() {
        controls = new PlayerControls.Input();
        rb = this.GetComponent<Rigidbody>();
        textBoxContent = new();
        meshRenderer = this.GetComponent<MeshRenderer>();
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

            if (GameManager.Instance.canClean) {

                if (!playerStats.CheckToilet(5)) {
                    sendTextPopUp?.Invoke(textBoxContent.GetRandomToiletText());
                    return;
                }

                if (!playerStats.CheckHunger(5)) {
                    sendTextPopUp?.Invoke(textBoxContent.GetRandomHungerText());
                    return;
                }

                cameraManager.SwapCameras();
                if (GameManager.Instance.currentGamestate == GameManager.GameState.movingLift) {
                    GameManager.Instance.currentGamestate = GameManager.GameState.cleaning;
                    liftVisuals.SetActive(false);
                    windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = true;
                } else if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning) {
                    if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = false;
                    GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                    liftVisuals.SetActive(true);
                }
                return;
            }

            if (GameManager.Instance.canTakeBreak) {
                cameraManager.SwapCameras();
                if (GameManager.Instance.currentGamestate == GameManager.GameState.movingLift) {
                    GameManager.Instance.currentGamestate = GameManager.GameState.takingBreak;
                    liftVisuals.SetActive(false);
                    windowDetector.currentFocusedObject.GetComponent<BreakArea>().ToggleItem(true);
                } else if (GameManager.Instance.currentGamestate == GameManager.GameState.takingBreak) {
                    GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                    liftVisuals.SetActive(true);
                    windowDetector.currentFocusedObject.GetComponent<BreakArea>().ToggleItem(false);
                }
                return;
            }

            if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning) {
                cameraManager.SwapCameras();
                if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = false;
                GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                liftVisuals.SetActive(true);
                return;
            }

            /*
            if (GameManager.Instance.currentGamestate == GameManager.GameState.takingBreak) {
                cameraManager.SwapCameras();
                GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                return;
            }
            */

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
        bool isMovingDownward = moveInput.y < 0;
        float adjustedSpeed = isMovingDownward ? speed * downwardSpeedMultiplier : speed;
        rb.velocity = moveInput * adjustedSpeed * Time.fixedDeltaTime;

    }
}
