using System.Collections;
using UnityEngine;

public class UI_Transitions : MonoBehaviour {

    [SerializeField] private CanvasGroup blackBackgroundCG;

    private float transitionTime;

    private void Awake() {
        UIControls.OnGameStart += OnSceneTransitionTrigger;
        GameControllerUI.OnGameStart += OnSceneTransitionTrigger;
    }

    private void OnSceneTransitionTrigger(float transitionTime, bool fadeToBlack) {
        this.transitionTime = transitionTime;
        StartCoroutine(fadeToBlack ? BlackFadeIn() : BlackFadeAway());
    }

    public IEnumerator BlackFadeAway() {
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime) {
            elapsedTime += Time.deltaTime;
            float fadeAmount = elapsedTime / transitionTime;
            float newAlpha = Mathf.Lerp(1f, 0f, fadeAmount);
            blackBackgroundCG.alpha = newAlpha;
            yield return null;
        }
        blackBackgroundCG.alpha = 0f;
    }

    public IEnumerator BlackFadeIn() {
        float elapsedTime = 0f;
        while (elapsedTime < transitionTime) {
            elapsedTime += Time.unscaledDeltaTime;
            float fadeAmount = elapsedTime / transitionTime;
            float newAlpha = Mathf.Lerp(0f, 1f, fadeAmount);
            blackBackgroundCG.alpha = newAlpha;
            yield return null;
        }
        blackBackgroundCG.alpha = 1f;
    }

    private void OnDisable() {
        UIControls.OnGameStart -= OnSceneTransitionTrigger;
        GameControllerUI.OnGameStart -= OnSceneTransitionTrigger;
    }

}
