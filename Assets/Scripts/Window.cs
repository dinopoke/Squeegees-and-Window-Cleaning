using UnityEngine;
using UnityEngine.InputSystem;

public class CleanableWindow : MonoBehaviour {
    public Texture2D cleanTexture;
    public Texture2D dirtyTexture;
    public Shader cleanableWindowShader;
    public float cleaningSpeed = 0.5f; // Speed of the cleaning tool
    public float cursorSpeed = 0.5f; // Speed of cursor movement
    public int cleanRadius = 20;

    private Texture2D maskTexture;
    private Material material;
    private Camera mainCamera;
    private PlayerControls.Input controls;
    private Vector2 moveInput;
    private bool isCleaning;
    private Vector2 cursorPosition;

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
        controls.Disable();
    }

    void Start() {
        mainCamera = Camera.main;

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

    void Update() {
        // Update cursor position based on the analog stick input
        cursorPosition += moveInput * cursorSpeed * Time.deltaTime;

        // Clamp cursor position to stay within the window bounds
        cursorPosition.x = Mathf.Clamp01(cursorPosition.x);
        cursorPosition.y = Mathf.Clamp01(cursorPosition.y);

        if (isCleaning) {
            CleanAt(cursorPosition);
        }
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
