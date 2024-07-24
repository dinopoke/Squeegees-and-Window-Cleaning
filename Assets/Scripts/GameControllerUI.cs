using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameControllerUI : MonoBehaviour
{
    private PlayerControls.Input controls;
    private float transitionTime = 1f;
    private bool gameStarted;

    public static event Action<float, bool> OnGameStart;

    private void Awake() {
        Invoke(nameof(Initialise), transitionTime + 1f);
    }

    private void Start() {
        Invoke(nameof(DelayTransition), .5f);
    }

    private void DelayTransition() {
        OnGameStart?.Invoke(transitionTime, false);
    }

    private void Initialise() {
        controls = new PlayerControls.Input();
        controls.Player.QuitGame.performed += StartGamePerformed;
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
            SceneManager.LoadScene("Title");
    }

    private void OnDisable() {
        controls.Disable();
    }
}
