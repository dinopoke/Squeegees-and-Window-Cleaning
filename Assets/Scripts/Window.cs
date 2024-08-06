using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CleanableWindow : MonoBehaviour {
    public Texture2D cleanTexture;
    public Texture2D dirtyTexture;
    public Shader cleanableWindowShader;
    public float cleaningSpeed = 0.5f; // Speed of the cleaning tool
    public float cursorSpeed = 0.5f; // Speed of cursor movement
    public int cleanRadius = 20;
    public float cleanlinessThreshold = 0.95f;
    private bool isWindowClean;
    private bool checkCleanliness;

    private Texture2D maskTexture;
    private Material material;
    private Camera mainCamera;
    private PlayerControls.Input controls;
    private Vector2 moveInput;
    private bool isCleaning;
    private Vector2 cursorPosition;

    public Outline windowOutline;
    public LayerMask raycastLayerMask;

    public static event Action<string> sendTextPopUp;

    private AudioSource windowAudioSource;
    public List<AudioClip> audioClips;

    void Awake() {
        controls = new PlayerControls.Input();
    }

    void OnEnable() {
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Clean.performed += ctx => isCleaning = true;
        controls.Player.Clean.canceled += ctx => isCleaning = false;
        controls.Enable();
    }

    void OnDisable() {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Clean.performed -= ctx => isCleaning = true;
        controls.Player.Clean.canceled -= ctx => isCleaning = false;
        controls.Disable();
        isCleaning = false;
        if (windowAudioSource.isPlaying) windowAudioSource.Stop();

    }

    void Start() {
        mainCamera = Camera.main;

        windowAudioSource = gameObject.AddComponent<AudioSource>();
        windowAudioSource.loop = true;

        // Initialize cursor position to the center of the window
        cursorPosition = new Vector2(0.5f, 0.5f);

        // Create the mask texture
        maskTexture = new Texture2D(dirtyTexture.width, dirtyTexture.height, TextureFormat.R8, false);
        for (int x = 0; x < maskTexture.width; x++) {
            for (int y = 0; y < maskTexture.height; y++) {
                maskTexture.SetPixel(x, y, Color.black);
            }
        }
        maskTexture.Apply();

        // Create the material with the shader and textures
        if (cleanableWindowShader == null) {
            cleanableWindowShader = Shader.Find("Custom/CleanableWindow");
        }

        material = new Material(cleanableWindowShader);
        material.SetTexture("_MainTex", dirtyTexture);
        material.SetTexture("_CleanTex", cleanTexture);
        material.SetTexture("_MaskTex", maskTexture);

        // Apply the material to the object
        GetComponent<Renderer>().material = material;
    }

    private void SqueegeeSound() {
        if (windowAudioSource.isPlaying) return;
        windowAudioSource.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
        windowAudioSource.Play();
    }

    private Vector2 adjustment = new(0.05f, 0.075f);
    private Vector2 offset = new(0.5f, 0.5f);

    void Update() {

        if (!isWindowClean) {

            Vector3 mousePos = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastLayerMask)) {
                if (hit.collider != null && hit.collider.gameObject == gameObject) {
                    Vector3 localHit = transform.InverseTransformPoint(hit.point);
                    //Vector3 normalizedHit = new Vector3(localHit.x / transform.localScale.x + 0.5f, localHit.y / transform.localScale.y + 0.5f, localHit.z / transform.localScale.z + 0.5f);
                    Vector3 normalizedHit = new Vector3(localHit.x * transform.localScale.x * adjustment.x, localHit.y * transform.localScale.y * adjustment.y, localHit.z * transform.localScale.z * adjustment.x);
                    //cursorPosition = new Vector2(localHit.z + adjustment.x, localHit.y + adjustment.y);
                    cursorPosition = new Vector2(normalizedHit.z + offset.x, normalizedHit.y + offset.y);
                }
            }

            /*

            // Update cursor position based on the analog stick input
            cursorPosition += moveInput * cursorSpeed * Time.deltaTime;

            // Clamp cursor position to stay within the window bounds
            cursorPosition.x = Mathf.Clamp01(cursorPosition.x);
            cursorPosition.y = Mathf.Clamp01(cursorPosition.y);

            */

            if (isCleaning) {
                SqueegeeSound();
                checkCleanliness = true;
                CleanAt(cursorPosition);
            }

            if (!isCleaning) {
                if (windowAudioSource.isPlaying) windowAudioSource.Stop();
                if (checkCleanliness) {
                    checkCleanliness = false;
                    float cleanliness = CalculateCleanliness();
                    if (cleanliness >= cleanlinessThreshold) {
                        GameManager.Instance.AddCleanWindow();
                        AutoCleanWindow();
                    }
                }
            }

            
        }
    }


    private float CalculateCleanliness() {
        // Retrieve all pixel data from the texture
        Color[] pixels = maskTexture.GetPixels();
        int cleanPixels = 0;
        int totalPixels = maskTexture.width * maskTexture.height;

        // Process the pixel data to count clean pixels
        for (int i = 0; i < pixels.Length; i++) {
            if (pixels[i].r > 0.5f) {
                cleanPixels++;
            }
        }

        // Log the number of clean pixels
        Debug.Log("Clean pixels: " + cleanPixels);

        return (float)cleanPixels / totalPixels;
    }

    void AutoCleanWindow() {
        isWindowClean = true;
        sendTextPopUp?.Invoke("Window Cleaned");
        GameManager.Instance.ReplaceWindowWithClean(this.transform.position);
        GameManager.Instance.canClean = false;
        Destroy(this.gameObject);        
    }


    void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    void CleanAt(Vector2 uv) {
        int x = (int)(uv.x * maskTexture.width);
        int y = (int)(uv.y * maskTexture.height);

        int radius = cleanRadius;
        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                int newX = Mathf.Clamp(x + i, 0, maskTexture.width - 1);
                int newY = Mathf.Clamp(y + j, 0, maskTexture.height - 1);
                maskTexture.SetPixel(newX, newY, Color.white);
            }
        }

        maskTexture.Apply();
        material.SetTexture("_MaskTex", maskTexture);
    }

    // Optional: Visualize the cursor position for debugging
    void OnGUI() {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 pixelPos = new Vector2(cursorPosition.x * Screen.width, Screen.height - cursorPosition.y * Screen.height);
        GUI.Box(new Rect(pixelPos.x - 5, pixelPos.y - 5, 10, 10), "");
    }
}
