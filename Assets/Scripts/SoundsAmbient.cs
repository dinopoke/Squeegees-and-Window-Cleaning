using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsAmbient : MonoBehaviour {

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;
    public bool loop = true;

    private Coroutine playCoroutine;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        playCoroutine = StartCoroutine(PlaySoundsSequentially());
    }

    private IEnumerator PlaySoundsSequentially() {
        do {
            foreach (AudioClip clip in audioClips) {
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }
        } while (loop);
    }

    public void StopPlayback() {
        if (playCoroutine != null) {
            StopCoroutine(playCoroutine);
            audioSource.Stop();
        }
    }
}