using UnityEngine.Audio;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;

    public void Play(AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
        Invoke(nameof(ReturnToPool), audioSource.clip.length + .25f);
    }

    private void ReturnToPool() {
        SoundPool.instance.ReturnSoundToPool(this);
    }

}
