using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager {

    public enum Sound {
        None,
        eat,
        toilet,
        liftmove,
        liftstop,


    }

    public static void PlaySound(Sound sound, Vector2 position) {

        //make a sound object pool + prefab
        //GameObject soundGameObject = new GameObject("Sound");
        //AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        if (sound == Sound.None) return;
        AudioClip audioClip = GetAudioClip(sound);
        PlaySound soundPlayer = SoundPool.instance.GetSoundPrefab();
        if (soundPlayer == null) return;
        soundPlayer.gameObject.transform.position = position;
        soundPlayer.gameObject.SetActive(true);
        soundPlayer.Play(audioClip);
        
    }

    public static AudioClip GetAudioClip(Sound sound) {
        foreach (AudioAssets.SoundAudioClip soundAudioClip in AudioAssets.instance.SoundAudioClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private static AudioClip GetGlobalAudioClip(Sound sound) {
        foreach (AudioAssets.SoundAudioClip soundAudioClip in AudioAssets.instance.GlobalSoundClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Global sound " + sound + " not found!");
        return null;
    }

}
