using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class UIControls : MonoBehaviour
{
    private PlayerControls.Input controls;
    private float transitionTime = 1f;
    private bool gameStarted;

    public static event Action<float, bool> OnGameStart;

    private void Awake() {
        Invoke(nameof(Initialise), transitionTime);
    }

    private void Start() {
        OnGameStart?.Invoke(transitionTime, false);
    }

    private void Initialise() {
        controls = new PlayerControls.Input();
        controls.Player.SwitchCamera.performed += StartGamePerformed;
        controls.Enable();
    }

    private void StartGamePerformed(InputAction.CallbackContext context) {
        if (!gameStarted) {
            gameStarted = true;
            OnGameStart?.Invoke(transitionTime, true);
            StartCoroutine(StartGameTransition());
        }
    }

    private IEnumerator StartGameTransition() {
            yield return new WaitForSeconds(transitionTime + .1f);
            SceneManager.LoadScene("GameScene");
    }

    private void OnDisable() {
        controls.Disable();
    }
}
