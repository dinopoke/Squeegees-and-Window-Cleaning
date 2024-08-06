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

    private bool isMoving;
    private bool hasStoppedSoundPlayed = true;

    [SerializeField] private GameObject liftVisuals;

    private Coroutine delayWindowActivation;


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

            moveInput = Vector2.zero;

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
                    if (delayWindowActivation != null) StopCoroutine(delayWindowActivation);
                    delayWindowActivation = StartCoroutine(ActivateWindowCleaning());
                } else if (GameManager.Instance.currentGamestate == GameManager.GameState.cleaning) {
                    if (delayWindowActivation != null) StopCoroutine(delayWindowActivation);
                    if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = false;
                    GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                    liftVisuals.SetActive(true);
                }
                return;
            }

            if (GameManager.Instance.canTakeBreak) {
                cameraManager.SwapCameras();
                if (delayWindowActivation != null) StopCoroutine(delayWindowActivation);
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
                if (delayWindowActivation != null) StopCoroutine(delayWindowActivation);
                cameraManager.SwapCameras();
                if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = false;
                GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                liftVisuals.SetActive(true);
                return;
            } else if (GameManager.Instance.currentGamestate != GameManager.GameState.movingLift) {
                if (delayWindowActivation != null) StopCoroutine(delayWindowActivation);
                cameraManager.SwapToBuildingCam();
                if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = false;
                GameManager.Instance.currentGamestate = GameManager.GameState.movingLift;
                liftVisuals.SetActive(true);

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

    private IEnumerator ActivateWindowCleaning() {
        yield return new WaitForSeconds(2.1f);
        if (windowDetector.currentFocusedObject != null) windowDetector.currentFocusedObject.GetComponent<CleanableWindow>().enabled = true;
    }



    void LiftMove(InputAction.CallbackContext context) {
        if (GameManager.Instance.currentGamestate == GameManager.GameState.movingLift) {
            moveInput = context.ReadValue<Vector2>();

            if (moveInput != Vector2.zero) {
                if (!isMoving) {
                    isMoving = true;
                    if (hasStoppedSoundPlayed) liftAudioSource.Stop();
                    if (!liftAudioSource.isPlaying) {
                        liftAudioSource.clip = AudioManager.GetAudioClip(AudioManager.Sound.liftmove);
                        float randomStartTime = UnityEngine.Random.Range(0, liftAudioSource.clip.length);
                        liftAudioSource.time = randomStartTime;
                        liftAudioSource.loop = true;
                        //liftAudioSource.volume = 0.35f;
                        liftAudioSource.Play();
                        hasStoppedSoundPlayed = false;
                    }
                }
            } else {
                if (isMoving) {
                    isMoving = false;
                    if (liftAudioSource.isPlaying) liftAudioSource.Stop();
                    if (!hasStoppedSoundPlayed) {
                        hasStoppedSoundPlayed = true;
                        /*
                        liftAudioSource.clip = AudioManager.GetAudioClip(AudioManager.Sound.liftstop);
                        liftAudioSource.loop = false;
                        liftAudioSource.volume = 0.2f;
                        liftAudioSource.Play();
                        */
                        AudioManager.PlaySound(AudioManager.Sound.liftstop, new(32, this.transform.position.y + 28));
                    }
                }
            }
        } else {
            moveInput = Vector2.zero;
        }
    }
    private AudioSource liftAudioSource;
    // Start is called before the first frame update
    void Start()
    {

        liftAudioSource = gameObject.GetComponent<AudioSource>();
        liftAudioSource.clip = AudioManager.GetAudioClip(AudioManager.Sound.liftmove);
        liftAudioSource.loop = true;

    }

    // Update is called once per frame
    void FixedUpdate() {

        if (GameManager.Instance.currentGamestate == GameManager.GameState.movingLift) {
            bool isMovingDownward = moveInput.y < 0;
            float adjustedSpeed = isMovingDownward ? speed * downwardSpeedMultiplier : speed;
            rb.velocity = new Vector2(moveInput.x * speed, moveInput.y * adjustedSpeed) * Time.fixedDeltaTime;
        } else {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
            if (liftAudioSource.isPlaying) {
                liftAudioSource.Stop();
                AudioManager.PlaySound(AudioManager.Sound.liftstop, new(32, this.transform.position.y + 28));
                hasStoppedSoundPlayed = false;
            }
        }
    }
}
