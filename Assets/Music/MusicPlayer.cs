using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instance;

    private AudioSource audioSource;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip j1;
    [SerializeField] private AudioClip j2;

    private Coroutine playCoroutine;

    private void Awake() {
        instance = this;
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.Play();
    }

    public void playJingle1() {
        audioSource.clip = j1;
        playCoroutine = StartCoroutine(PlaySoundsSequentially());
    }
    
    public void playJingle2() {
        audioSource.clip = j2;
        playCoroutine = StartCoroutine(PlaySoundsSequentially());
    }

    private IEnumerator PlaySoundsSequentially() {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = music;
        audioSource.Play();
    }

}
